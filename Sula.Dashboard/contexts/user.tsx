import setLanguage from 'next-translate/setLanguage';
import { useRouter } from 'next/dist/client/router';
import React, { createContext, ReactElement, useEffect } from 'react';

import { Language, User } from '../interfaces';
import { useAuthentication } from './authentication';

interface Props {
  children: React.ReactNode;
}

export interface UserContextType {
  user?: User;
}

// eslint-disable-next-line @typescript-eslint/no-non-null-assertion
const UserContext = createContext<UserContextType>(undefined!);

export default function UserProvider(props: Props): ReactElement {
  const { pathname, push } = useRouter();

  const { user, error, isLoading } = useAuthentication();

  useEffect(() => {
    if (error) {
      push('/unauthorized');
      return;
    }

    if (pathname !== '/login' && !user && !isLoading) {
      push('/login');
    }

    if (pathname === '/login' && user) {
      push('/');
    }

    if (user) {
      setLanguage(Language[user.settings.language]);
    }
  }, [pathname, user, isLoading, error]);

  return <UserContext.Provider value={{ user }}>{props.children}</UserContext.Provider>;
}

const useUser = (): UserContextType => React.useContext(UserContext);

export { UserProvider, useUser };
