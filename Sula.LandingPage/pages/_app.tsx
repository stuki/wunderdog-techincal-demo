import React from 'react';
import { AppProps } from 'next/app';

import '../styles/index.css';

const MyApp = ({ Component, pageProps }: AppProps): JSX.Element => <Component {...pageProps} />;

export default MyApp;
