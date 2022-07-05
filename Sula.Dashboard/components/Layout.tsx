import Link from 'next/link';
import Head from 'next/head';
import React from 'react';
import { Navigation } from './Organisms';

type Props = {
  title?: string;
};

const Layout: React.FunctionComponent<Props> = ({ children, title }) => (
  <>
    <Head>
      <title>Sula — {title}</title>
      <meta charSet="utf-8" />
      <meta name="viewport" content="initial-scale=1.0, width=device-width" />
      <link href="https://rsms.me/inter/inter.css" rel="stylesheet" />
    </Head>
    <div className="w-full h-full md:grid md:grid-cols-1 flex flex-col md:grid-rows-layout">
      <header className="flex md:items-center py-3 md:justify-center color-darker">
        <Link href="/">
          <a className={'pl-5 font-bold md:text-5xl text-3xl logo-gradient'}>sula</a>
        </Link>
      </header>
      <Navigation></Navigation>
      <main className="overflow-hidden overflow-y-scroll w-full">
        <div className="px-2 md:px-24 color-darker pb-10 min-h-fullIsh">{children}</div>
        <footer className="h-32"></footer>
      </main>
    </div>
  </>
);

export default Layout;
