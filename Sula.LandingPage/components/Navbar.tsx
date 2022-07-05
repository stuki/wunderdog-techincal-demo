import * as React from 'react';
import Link from 'next/link';
import setLanguage from 'next-translate/setLanguage';
import useTranslation from 'next-translate/useTranslation';
import { useRouter } from 'next/dist/client/router';

const NavBar: React.FunctionComponent = () => {
  const { t, lang } = useTranslation('common');
  const { route } = useRouter();

  return (
    <div className={'flex top-0 h-20 justify-between items-center w-5/6 mx-auto'}>
      <Link href="/">
        <a className={'text-3xl font-bold logo-gradient'}>sula</a>
      </Link>
      <ul className={'list-none flex flex-row lg:w-1/3 md:w-1/2 w-2/3 opacity-70 justify-between'}>
        <li>
          <Link href="/">
            <a className={`hover:opacity-50 cursor-pointer ${!route.includes('faq') ? 'underline opacity-50' : ''}`}>
              {t('home')}
            </a>
          </Link>
        </li>
        <li>
          <Link href="/faq">
            <a className={`hover:opacity-50 cursor-pointer ${route.includes('faq') ? 'underline opacity-50' : ''}`}>
              {t('faq')}
            </a>
          </Link>
        </li>
        <li>
          <span
            className={`hover:opacity-50 cursor-pointer ${lang === 'en' ? 'underline opacity-70' : ''}`}
            onClick={async () => await setLanguage('en')}>
            en
          </span>
          <span className={'mx-3'}>/</span>
          <span
            className={`hover:opacity-50 cursor-pointer ${lang === 'fi' ? 'underline opacity-70' : ''}`}
            onClick={async () => await setLanguage('fi')}>
            fi
          </span>
        </li>
      </ul>
    </div>
  );
};

export default NavBar;
