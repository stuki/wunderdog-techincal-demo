import React from 'react';
import { AppProps } from 'next/app';

import '../styles/index.css';
import { AppProviders } from '../contexts/AppProviders';

const App = ({ Component, pageProps }: AppProps): JSX.Element => (
  <AppProviders>
    <Component {...pageProps} />
  </AppProviders>
);

export default App;
