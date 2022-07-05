import * as React from 'react';
import useTranslation from 'next-translate/useTranslation';
import Trans from 'next-translate/Trans';

const Hero: React.FunctionComponent = () => {
  const { t } = useTranslation('common');

  return (
    <main className={'flex my-10 justify-between items-center w-11/12 mx-auto lg:flex-row flex-col'}>
      <div className={'pr-5 order-2 lg:order-1'}>
        <h2 className={'text-4xl lg:text-5xl font-bold mb-10 opacity-90'}>
          <Trans i18nKey="common:hero" components={[<span className={'inline-block'} />]} />
        </h2>
        <ul className={'text-lg opacity-70'}>
          <li className={'py-3'}>{t('sellingPoints.1')}</li>
          <li className={'py-3'}>{t('sellingPoints.2')}</li>
          <li className={'py-3'}>{t('sellingPoints.3')}</li>
          <li className={'py-3'}>{t('sellingPoints.4')}</li>
          <li className={'py-3'}>{t('sellingPoints.5')}</li>
          <li className={'py-3'}>{t('sellingPoints.6')}</li>
        </ul>
      </div>
      <div
        className={
          'h-iphone min-w-iphone bg-white rounded-3xl shadow-2xl phone-img mb-10 order-1 lg:mb-0 lg:order-2'
        }></div>
    </main>
  );
};
export default Hero;
