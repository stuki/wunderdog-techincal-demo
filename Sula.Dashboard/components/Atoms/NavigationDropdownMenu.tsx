import { Menu, Transition } from '@headlessui/react';
import classNames from 'classnames';
import useTranslation from 'next-translate/useTranslation';
import React, { Fragment } from 'react';
import { useAuthentication } from '../../contexts/authentication';
import { ChevronDownIcon } from '@heroicons/react/solid';
import { LogoutIcon } from '@heroicons/react/outline';

export function NavigationDropdownMenu() {
  const { user, logout } = useAuthentication();
  const { t } = useTranslation('common');

  return (
    <Menu as="div" className="relative text-left md:col-start-12 self-end md:self-auto hidden md:inline-block">
      {({ open }) => (
        <>
          <Menu.Button className="inline-flex justify-center w-full rounded-md px-4 py-2 color-dark text-sm font-medium text-gray-200 hover:opacity-90">
            {user?.email}
            <ChevronDownIcon className="w-5 h-5 ml-2 -mr-1 text-violet-200 hover:text-violet-100" aria-hidden="true" />
          </Menu.Button>

          <Transition
            show={open}
            as={Fragment}
            enter="transition ease-out duration-100"
            enterFrom="transform opacity-0 scale-95"
            enterTo="transform opacity-100 scale-100"
            leave="transition ease-in duration-75"
            leaveFrom="transform opacity-100 scale-100"
            leaveTo="transform opacity-0 scale-95">
            <Menu.Items static className="origin-top-right absolute right-0 mt-2 w-56 rounded-md shadow-lg color-dark">
              <div className="py-1">
                <Menu.Item>
                  {({ active }) => (
                    <a
                      href="/account"
                      className={classNames(
                        active ? 'bg-gray-100 text-gray-900' : 'text-gray-200',
                        'block px-4 py-2 text-sm'
                      )}>
                      {t('account')}
                    </a>
                  )}
                </Menu.Item>
                <Menu.Item>
                  {({ active }) => (
                    <a
                      href="#"
                      className={classNames(
                        active ? 'bg-gray-100 text-gray-900' : 'text-gray-200',
                        'block px-4 py-2 text-sm'
                      )}>
                      {t('support')}
                    </a>
                  )}
                </Menu.Item>
                <Menu.Item>
                  {({ active }) => (
                    <a
                      href="#"
                      className={classNames(
                        active ? 'bg-gray-100 text-gray-900' : 'text-gray-200',
                        'block px-4 py-2 text-sm'
                      )}>
                      {t('terms')}
                    </a>
                  )}
                </Menu.Item>
                <form method="POST" action="#" onSubmit={logout}>
                  <Menu.Item>
                    {({ active }) => (
                      <button
                        type="submit"
                        className={classNames(
                          active ? 'bg-gray-100 text-gray-900' : 'text-gray-200',
                          'w-full text-left px-4 py-2 text-sm flex flex-row'
                        )}>
                        {t('signout')}
                        <LogoutIcon className="w-5 h-5 ml-2 text-gray-500" />
                      </button>
                    )}
                  </Menu.Item>
                </form>
              </div>
            </Menu.Items>
          </Transition>
        </>
      )}
    </Menu>
  );
}
