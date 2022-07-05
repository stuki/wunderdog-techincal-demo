import * as React from 'react';
import Head from 'next/head';
import NavBar from './Navbar';

type Props = {
  title?: string;
};

const Layout: React.FunctionComponent<Props> = ({ children, title = 'Sula.app' }) => (
  <div className={'w-3/5 m-auto justify-center'}>
    <Head>
      <title>{title}</title>
      <meta charSet="utf-8" />
      <meta name="viewport" content="initial-scale=1.0, width=device-width" />
    </Head>
    <NavBar />
    {children}
  </div>
);

export default Layout;
