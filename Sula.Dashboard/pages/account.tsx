import React, { useState } from 'react';
import Layout from '../components/Layout';
import { NextPage } from 'next';
import { useQuery } from 'react-query';
import { getBillingPortalUrl, getUser, updateUser } from '../services/api';
import { InputField, SelectField, Title } from '../components/Atoms';
import { Field, Form, Formik } from 'formik';
import { Language, User } from '../interfaces';
import useTranslation from 'next-translate/useTranslation';
import setLanguage from 'next-translate/setLanguage';

enum Page {
  Account,
  Settings,
}

const AccountPage: NextPage = () => {
  const { t } = useTranslation('account');
  const userQuery = useQuery('getUser', getUser);
  const billingUrlQuery = useQuery('getBillingPortalUrl', getBillingPortalUrl);
  const [page, setPage] = useState(Page.Account);

  async function handleSubmit(user: User) {
    const { succeeded } = await updateUser(user);

    if (succeeded) {
      setLanguage(Language[user.settings.language]);
    }
  }

  function renderView() {
    switch (page) {
      case Page.Account:
        return <AccountView />;
      case Page.Settings:
        return <SettingsView />;
    }
  }

  return (
    <Layout title={t('common:account')}>
      <Title>{t('common:account')}</Title>
      <div className="md:grid md:grid-cols-12 color-dark rounded-xl md:divide-x divide-y md:divide-y-0 divide-gray-600">
        <div className="md:col-span-2 flex flex-row">
          <ul>
            <li
              onClick={() => setPage(Page.Account)}
              className="p-5 hover:opacity-80 cursor-pointer float-left md:float-none active:bg-green-900">
              {t('account')}
            </li>
            <li
              onClick={() => setPage(Page.Settings)}
              className="p-5 hover:opacity-80 cursor-pointer float-left md:float-none active:bg-green-900">
              {t('settings')}
            </li>
            <li className="p-5 hover:opacity-80 cursor-pointer float-left md:float-none active:bg-green-900">
              <a href={billingUrlQuery.data?.url} className="flex items-center">
                {t('billing')}
                <svg width="14" height="14" viewBox="0 0 24 24" className="ml-2">
                  <g stroke-width="2.1" stroke="white" fill="none" stroke-linecap="round" stroke-linejoin="round">
                    <polyline points="17 13.5 17 19.5 5 19.5 5 7.5 11 7.5"></polyline>
                    <path d="M14,4.5 L20,4.5 L20,10.5 M20,4.5 L11,13.5"></path>
                  </g>
                </svg>
              </a>
            </li>
          </ul>
        </div>
        <div className="col-span-10 px-5 pb-10 overflow-hidden overflow-y-scroll">
          {userQuery.data && (
            <Formik initialValues={userQuery.data} onSubmit={handleSubmit}>
              {() => <Form>{renderView()}</Form>}
            </Formik>
          )}
        </div>
      </div>
    </Layout>
  );
};

export default AccountPage;

function AccountView() {
  const { t } = useTranslation('account');
  return (
    <>
      <h2 className="text-md my-5 uppercase font-bold">{t('account')}</h2>
      <div className="mb-5 md:w-1/3">
        <label htmlFor="email">{t('email')}</label>
        <Field type="email" name="email" component={InputField} disabled />
      </div>
      <div className="mb-5 md:w-1/3">
        <label htmlFor="phoneNumber">{t('phone')}</label>
        <Field type="tel" name="phoneNumber" component={InputField} disabled />
      </div>
      {/* <div className="mb-5 w-1/3">
        <label htmlFor="password">{t('change_password')}</label>
        <Field type="password" name="password" component={InputField} />
      </div>
      <div className="mb-5 w-1/3">
        <label htmlFor="password">{t('confirm_password')}</label>
        <Field type="password" name="password" component={InputField} />
      </div>
      <button type="submit" className="button-gradient px-10 py-4 mt-10 font-semibold text-white rounded-lg">
        {t('save')}
      </button> */}
    </>
  );
}

function SettingsView() {
  const { t } = useTranslation('account');
  return (
    <>
      <h2 className="text-md my-5 uppercase font-bold">{t('settings')}</h2>
      <div className="mb-5 md:w-1/3">
        <label htmlFor="Language">{t('language')}</label>
        <Field as="select" name="settings.language" component={SelectField}>
          <option value="0">{t('english')}</option>
          <option value="1">{t('finnish')}</option>
        </Field>
      </div>
      <button type="submit" className="button-gradient px-10 py-4 mt-10 font-semibold text-white rounded-lg">
        {t('save')}
      </button>
    </>
  );
}
