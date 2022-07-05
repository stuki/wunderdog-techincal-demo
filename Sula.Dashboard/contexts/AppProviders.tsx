import React, { ReactElement } from 'react';
import { QueryClient, QueryClientProvider } from 'react-query';

import { AuthenticationProvider } from './authentication';
import UserProvider from './user';

interface Props {
  children: React.ReactNode;
}

const queryClient = new QueryClient();

export function AppProviders(props: Props): ReactElement {
  return (
      <QueryClientProvider client={queryClient}>
        <AuthenticationProvider>
          <UserProvider>{props.children}</UserProvider>
        </AuthenticationProvider>
      </QueryClientProvider>
  );
}
