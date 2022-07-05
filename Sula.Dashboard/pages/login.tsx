import { NextPage } from 'next';
import useTranslation from 'next-translate/useTranslation';
import Head from 'next/head';
import Link from 'next/link';
import React from 'react';
import { LoginSection } from '../components/Organisms';

const LoginPage: NextPage = () => {
  const { t } = useTranslation('common');

  return (
    <>
      <Head>
        <title>Sula — {t('login')}</title>
        <meta charSet="utf-8" />
        <meta name="viewport" content="initial-scale=1.0, width=device-width" />
      </Head>
      <div className="w-full md:h-full grid md:grid-cols-3 grid-cols-1">
        <span className="background-gradient col-span-2 p-10">
          <Link href="https://sula.app/">
            <a className={'font-bold text-7xl logo-gradient'}>sula</a>
          </Link>
        </span>
        <LoginSection />
      </div>
    </>
  );
};

export default LoginPage;
