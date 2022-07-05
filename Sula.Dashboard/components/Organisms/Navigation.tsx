import classNames from 'classnames';
import useTranslation from 'next-translate/useTranslation';
import React, { ReactElement, useState } from 'react';
import { NavigationDropdownMenu, NavigationItem } from '../Atoms';
import { UserIcon, DeviceTabletIcon, BellIcon, HomeIcon, LogoutIcon } from '@heroicons/react/outline';

export function Navigation(): ReactElement {
  const { t } = useTranslation('common');

  const [isOpen, setIsOpen] = useState(false);

  return (
    <div
      className={classNames(
        'md:grid md:grid-cols-12 w-full md:px-24 py-3 md:py-0 color-dark flex',
        isOpen ? 'flex-col' : 'items-center'
      )}>
      <button
        className={classNames(
          'md:hidden w-8 h-8 text-gray-200 p-1 ml-2 focus:outline-none',
          isOpen ? 'self-start' : ''
        )}
        onClick={() => setIsOpen(!isOpen)}>
        <svg fill="currentColor" viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg">
          <path
            fill-rule="evenodd"
            d="M3 5a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zM3 10a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zM3 15a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1z"
            clip-rule="evenodd"></path>
        </svg>
      </button>
      <div className={classNames('md:col-span-4 flex-col md:flex-row md:flex', isOpen ? 'flex' : 'hidden')}>
        <NavigationItem target="/" title={t('dashboard')}>
          <HomeIcon className="w-5 h-5 text-gray-500" />
        </NavigationItem>
        <NavigationItem target="/alerts" title={t('alerts')}>
          <BellIcon className="w-5 h-5 text-gray-500" />
        </NavigationItem>
        <NavigationItem target="/sensors" title={t('sensors')}>
          <DeviceTabletIcon className="w-5 h-5 text-gray-500" />
        </NavigationItem>
        <NavigationItem target="/account" title={t('account')}>
          <UserIcon className="w-5 h-5 text-gray-500" />
        </NavigationItem>
        {isOpen && (
          <button className={'flex flex-row items-center h-full text-sm hover:opacity-80 mr-10 md:p-0 py-2 px-4'}>
            <LogoutIcon className="w-5 h-5 text-gray-500" />
            <span className={'pl-2'}>{t('signout')}</span>
          </button>
        )}
      </div>
      <NavigationDropdownMenu />
    </div>
  );
}
