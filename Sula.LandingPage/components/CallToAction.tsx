import useTranslation from 'next-translate/useTranslation';
import * as React from 'react';
import { FormEvent, useEffect, useRef } from 'react';
import Loader from 'react-loader-spinner';

const CallToAction: React.FunctionComponent = () => {
  const { t } = useTranslation('common');
  const [email, setEmail] = React.useState<string>('');
  const [isValid, setIsValid] = React.useState(false);
  const [emailSent, setEmailSent] = React.useState(false);
  const [success, setSuccess] = React.useState<boolean>();

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  const ref = useRef<any>();

  const url = `${process.env.NEXT_PUBLIC_API_URL}${process.env.NEXT_PUBLIC_FUNCTION_CODE}`;

  async function handleSubmit(event: FormEvent) {
    event.preventDefault();

    setEmailSent(true);

    try {
      const response = await fetch(url + '&email=' + email, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
      });

      setSuccess(response.ok);

      if (!response.ok) {
        setEmailSent(false);
      }
    } catch (error) {
      setSuccess(false);
      setEmailSent(false);
    }
  }

  useEffect(() => {
    if (ref && ref.current) {
      setIsValid(ref.current.checkValidity());
    }
  }, [email]);

  function renderButtonText() {
    if (!emailSent) {
      return t('notify');
    }

    if (success) {
      return t('sent');
    }

    return (
      <div className="m-auto w-5">
        <Loader type="TailSpin" color="#FFFF" height={24} width={24} />
      </div>
    );
  }

  return (
    <footer className={'lg:mt-20 pb-10'}>
      <div className={'text-center'}>
        <h3 className={'text-3xl font-bold launch-gradient mb-8 mt-8'}>{t('launch')}</h3>
        <p className={'text-lg lg:text-2xl opacity-90'}>{t('cta')}</p>
      </div>
      <form ref={ref} onSubmit={handleSubmit} className={'flex justify-center mt-5 flex-col lg:flex-row'}>
        <input
          type="email"
          required
          placeholder={t('email')}
          value={email}
          onChange={(event) => setEmail(event.target.value)}
          className={
            'form-gradient text-center p-4 rounded-lg mb-3 lg:mb-0 lg:mr-3 lg:w-96 font-medium text-sm tracking-wide'
          }
          disabled={success && emailSent}
        />
        <button
          disabled={!isValid || emailSent}
          className={
            'button-gradient text-center py-4 px-10 rounded-lg \
          font-medium text-sm tracking-wide cursor-pointer transition-all \
          duration-200 hover:shadow-2xl hover:opacity-90 disabled:opacity-50'
          }>
          {renderButtonText()}
        </button>
      </form>
    </footer>
  );
};

export default CallToAction;
