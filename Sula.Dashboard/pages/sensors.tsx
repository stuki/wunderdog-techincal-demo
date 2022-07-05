import * as React from 'react';
import Layout from '../components/Layout';
import { NextPage } from 'next';
import { Title } from '../components/Atoms';
import useTranslation from 'next-translate/useTranslation';
import { useUser } from '../contexts/user';
import { SensorsSection } from '../components/Organisms';

const IndexPage: NextPage = () => {
  const { t } = useTranslation('common');
  const { user } = useUser();
  const sensors = user?.sensors;

  return (
    <Layout title={t('sensors')}>
      <Title>{t('sensors')}</Title>
      <div>{sensors && <SensorsSection sensors={sensors}></SensorsSection>}</div>
    </Layout>
  );
};

export default IndexPage;
