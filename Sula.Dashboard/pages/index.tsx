import * as React from 'react';
import Layout from '../components/Layout';
import { NextPage } from 'next';
import { useQuery } from 'react-query';
import { getAlerts, getSensorMinMaxData } from '../services/api';
import { AlertSection, GraphSection, LoadingView } from '../components/Organisms';
import { SmallCard, Title } from '../components/Atoms';
import useTranslation from 'next-translate/useTranslation';
import { useUser } from '../contexts/user';

const IndexPage: NextPage = () => {
  const { t } = useTranslation('common');
  const sensorsQuery = useQuery('sensorMinMaxData', getSensorMinMaxData);
  const alertsQuery = useQuery('alerts', getAlerts);
  const { user } = useUser();

  const isLoading = sensorsQuery.isLoading && alertsQuery.isLoading;

  return (
    <Layout title={t('dashboard')}>
      <Title>{t('dashboard')}</Title>
      {isLoading && <LoadingView />}
      {!isLoading && (
        <div className="grid md:grid-cols-12 gap-4">
          {!alertsQuery.isError && (
            <>
              <div className="md:col-span-8 hidden md:block">
                <AlertSection data={alertsQuery.data} detailed={false} showTitle={true}></AlertSection>
              </div>
              <div className="grid grid-cols-2 gap-2 md:hidden">
                <SmallCard value={user?.sensors.length ?? 0} title={t('sensors')} url="/sensors" />
                <SmallCard
                  value={alertsQuery.data?.length ?? 0}
                  title={t('alerts')}
                  color={alertsQuery.data?.length && alertsQuery.data?.length > 0 ? 'text-red-400' : ''}
                  url="/alerts"
                />
              </div>
            </>
          )}
          {!sensorsQuery.isError && <GraphSection cluster={sensorsQuery.data}></GraphSection>}
        </div>
      )}
    </Layout>
  );
};

export default IndexPage;
