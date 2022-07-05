import * as React from 'react';
import Layout from '../components/Layout';
import { NextPage } from 'next';
import { useQuery } from 'react-query';
import { getAlerts } from '../services/api';
import { AlertSection } from '../components/Organisms';
import { Title } from '../components/Atoms';
import useTranslation from 'next-translate/useTranslation';

const IndexPage: NextPage = () => {
  const { t } = useTranslation('common');
  const { data, isLoading, isError } = useQuery('alerts', getAlerts);

  return (
    <Layout title={t('alerts')}>
      <Title>{t('alerts')}</Title>
      <div>{!isLoading && !isError && <AlertSection data={data} detailed={true}></AlertSection>}</div>
    </Layout>
  );
};

export default IndexPage;
