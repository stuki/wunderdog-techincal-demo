import React, { createContext, ReactElement, useContext, useEffect, useState } from 'react';

import { User } from '../interfaces';
import { getToken, getUser, removeClientToken, setClientToken, updateToken } from '../services/api';
import { getTokenFromStorage, removeTokenFromStorage, storeTokenToStorage } from '../services/storage';

interface Props {
  children?: React.ReactNode;
}

export interface AuthContextType {
  user?: User;
  error?: any;
  isLoading: boolean;
  login: (credentials: { email: string; password: string }) => Promise<void>;
  logout: () => void;
}

// eslint-disable-next-line @typescript-eslint/no-non-null-assertion
export const AuthenticationContext = createContext<AuthContextType>(undefined!);

function AuthenticationProvider(props: Props): ReactElement {
  const [user, setUser] = useState<User | undefined>(undefined);
  const [error, setError] = useState<any>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const token = getTokenFromStorage();

    if (token === null) {
      setIsLoading(false);
      return;
    }

    updateToken(token.refresh_token)
      .then(storeTokenToStorage)
      .catch(removeTokenFromStorage)
      .finally(() => {
        setClientToken();
        getUser()
          .then(setUser)
          .finally(() => setIsLoading(false));
      });
  }, []);

  const login = async (credentials: { email: string; password: string }) => {
    try {
      setIsLoading(true);
      setError(null);
      setUser(undefined);

      const token = await getToken(credentials);

      storeTokenToStorage(token);

      setClientToken().then(() => {
        getUser()
          .then(setUser)
          .catch()
          .finally(() => setIsLoading(false));
      });
    } catch (error) {
      setError(error);
      setIsLoading(false);
    }
  };

  const logout = () => {
    removeTokenFromStorage();
    removeClientToken();
    setUser(undefined);
  };

  return (
    <AuthenticationContext.Provider value={{ user, error, isLoading, login, logout }}>
      {props.children}
    </AuthenticationContext.Provider>
  );
}

const useAuthentication = (): AuthContextType => useContext(AuthenticationContext);
export { AuthenticationProvider, useAuthentication };
