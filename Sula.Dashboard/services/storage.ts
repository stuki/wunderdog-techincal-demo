import { TokenResponse } from '../interfaces';

const userToken = 'token';

export const storeTokenToStorage = (tokenResponse: TokenResponse): void => {
  setItem(JSON.stringify(tokenResponse));
};

export const getTokenFromStorage = (): TokenResponse | null => {
  const storedValue = getItem();

  if (storedValue !== null) {
    return JSON.parse(storedValue);
  }

  return null;
};

export const removeTokenFromStorage = (): void => {
  removeItem();
};

const getItem = (key = userToken) => {
  return localStorage.getItem(key);
};

const setItem = (value: string, key = userToken) => {
  localStorage.setItem(key, value);
};

const removeItem = (key = userToken) => {
  localStorage.removeItem(key);
};
