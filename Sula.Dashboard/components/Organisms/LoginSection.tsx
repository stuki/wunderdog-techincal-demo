import useTranslation from 'next-translate/useTranslation';
import React, { ReactElement } from 'react';
import { useAuthentication } from '../../contexts/authentication';
import { ErrorMessage, Field, Form, Formik } from 'formik';
import { InputField } from '../Atoms';
import * as Yup from 'yup';
import Loader from 'react-loader-spinner';

export function LoginSection(): ReactElement {
  const { t } = useTranslation('common');

  const { login, isLoading } = useAuthentication();

  const SignupSchema = Yup.object().shape({
    email: Yup.string().email('Invalid email'),
    password: Yup.string(),
  });

  return (
    <div className="h-full w-full flex color-darker pt-10 md:pt-0">
      <section className="flex flex-col justify-center self-center m-auto md:px-5">
        <h1 className="text-4xl font-bold mb-10">Login</h1>
        <Formik
          initialValues={{ email: '', password: '' }}
          validationSchema={SignupSchema}
          onSubmit={(values, { setSubmitting }) => {
            login(values).then(() => setSubmitting(false));
          }}>
          {({ isSubmitting }) => (
            <Form>
              <Field type="email" name="email" placeholder={t('email_placeholder')} component={InputField} />
              <ErrorMessage name="email" component="div" />
              <Field type="password" name="password" placeholder={t('password_placeholder')} component={InputField} />
              <ErrorMessage name="password" component="div" />
              <div className="w-full flex justify-end mt-2">
                <a
                  href="https://dashboard.sula.app/forgot"
                  className="hover:opacity-80 text-gray-50 text-right text-sm">
                  {t('forgot_password')}
                </a>
              </div>
              <button
                type="submit"
                disabled={isSubmitting}
                className="button-gradient w-full px-4 py-4 mt-10 font-semibold text-white rounded-lg flex justify-center">
                {isLoading ? <Loader height="24" width="48" color="white" type="ThreeDots"></Loader> : t('login')}
              </button>
            </Form>
          )}
        </Formik>
        <div className="py-10">
          <p>Need sensors to monitor your spaces?</p>
          <a href="https://sula.app/" className="hover:opacity-80 text-gray-50">
            Check out our offerings here!
          </a>
        </div>
      </section>
    </div>
  );
}
