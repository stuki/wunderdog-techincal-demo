import axios from 'axios';
import createAuthRefreshInterceptor from 'axios-auth-refresh';
import queryString from 'query-string';

import { Limit, SensorDataCluster, TokenResponse, User, Sensor, SensorMinMaxDataCluster, Alert } from '../interfaces';

import { getTokenFromStorage, storeTokenToStorage } from './storage';

const ApiService = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL,
});

const refreshAuthLogic = async (failedRequest: any): Promise<any> => {
  const token = getTokenFromStorage();

  if (token === null) {
    return;
  }

  const response = await updateToken(token.refresh_token);

  storeTokenToStorage(response);
  failedRequest.response.config.headers['Authorization'] = 'Bearer ' + response.access_token;

  return Promise.resolve();
};

export const setClientToken = (): Promise<any> => {
  ApiService.interceptors.request.use((config) => {
    const token = getTokenFromStorage();

    if (token !== null) {
      config.headers.Authorization = `Bearer ${token.access_token}`;
    }

    return config;
  });

  return Promise.resolve();
};

export const removeClientToken = (): void => {
  ApiService.interceptors.request.use((config) => {
    config.headers.Authorization = '';
    return config;
  });
};

createAuthRefreshInterceptor(ApiService, refreshAuthLogic);

const scope = 'openid offline_access';
const audience = 'mobile';

export const getToken = (credentials: { email: string; password: string }): Promise<TokenResponse> =>
  ApiService.post<TokenResponse>(
    `/connect/token`,
    queryString.stringify({
      username: credentials.email,
      password: credentials.password,
      grant_type: 'password',
      scope,
      audience,
    }),
    {
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded',
      },
    }
  ).then((response) => response.data);

export const updateToken = (refresh_token: string): Promise<TokenResponse> =>
  ApiService.post<TokenResponse>(
    `/connect/token`,
    queryString.stringify({ refresh_token, grant_type: 'refresh_token', scope, audience }),
    {
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded',
      },
    }
  ).then((response) => response.data);

export const getUser = (): Promise<User> => ApiService.get<User>(`/api/v1/account`).then((response) => response.data);
export const updateUser = (user: User): Promise<{ succeeded: boolean }> =>
  ApiService.put<{ succeeded: boolean }>(`/api/v1/account`, user).then((response) => response.data);

export const sendPhoneConfirmationRequest = (phoneNumber: string): Promise<any> =>
  ApiService.post(`/api/v1/account/send`, { phoneNumber });
export const sendPhoneConfirmationToken = (phoneNumber: string, confirmationToken: string): Promise<any> =>
  ApiService.post(`/api/v1/phone/confirm`, { phoneNumber, confirmationToken });
export const getBillingPortalUrl = (): Promise<{ url: string }> =>
  ApiService.get<{ url: string }>(`/api/v1/billing`).then((response) => response.data);

export const getSensorData = (id?: string | string[] | undefined): Promise<SensorDataCluster[]> => {
  let url = '/api/v1/sensor/data';

  if (id) {
    url = url + '/' + id;
  }

  return ApiService.get<SensorDataCluster[]>(url).then((response) => response.data);
};
export const getSensorMinMaxData = (): Promise<SensorMinMaxDataCluster[]> =>
  ApiService.get<SensorMinMaxDataCluster[]>(`/api/v1/sensor/data/minmax`).then((response) => response.data);
export const updateSensor = (sensor: Sensor): Promise<Sensor> =>
  ApiService.put<Sensor>(`/api/v1/sensor/${sensor.id}`, sensor).then((response) => response.data);

export const addLimit = (sensor: Sensor, limit: Limit): Promise<Limit> =>
  ApiService.post<Limit>(`/api/v1/sensor/${sensor.id}/limit`, limit).then((response) => response.data);
export const updateLimit = (sensor: Sensor, limit: Limit): Promise<Limit> =>
  ApiService.put<Limit>(`/api/v1/sensor/${sensor.id}/limit/${limit.id}`, limit).then((response) => response.data);

export const getAlerts = (): Promise<Alert[]> =>
  ApiService.get<Alert[]>('/api/v1/alerts').then((response) => response.data);

export const getZones = (): Promise<any[]> => ApiService.get<any[]>('/api/v1/zones').then((response) => response.data);

export default ApiService;
