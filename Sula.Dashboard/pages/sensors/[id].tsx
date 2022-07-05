import { useRouter } from 'next/router';
import React from 'react';
import { useQuery } from 'react-query';
import { ResponsiveLineGraph, SmallCard, Title } from '../../components/Atoms';
import Layout from '../../components/Layout';
import { Card } from '../../components/Molecules';
import { AlertSection, LoadingView } from '../../components/Organisms';
import { DataType } from '../../interfaces';
import { getAlerts, getSensorData } from '../../services/api';

export default function Sensor() {
  const router = useRouter();
  const { id } = router.query;
  const { data, isLoading } = useQuery(['sensorData', id], () => getSensorData(id));
  const alertsQuery = useQuery('getAlerts', getAlerts);

  const name = data ? data[0].sensor.name ?? data[0].sensor.id : '';

  if (isLoading) {
    return <LoadingView></LoadingView>;
  }

  return (
    <Layout title={name}>
      <Title>{name}</Title>
      <div>
        <div className="grid md:grid-cols-6 grid-cols-2 gap-4 mb-5">
          {data?.map((cluster) => (
            <>
              <SmallCard value={cluster.latestEntry.value} title={`Current ${DataType[cluster.dataType]}`} />
              <SmallCard
                value={Math.max(...cluster.data.map((sensorData) => sensorData.value))}
                title={`Highest ${DataType[cluster.dataType]}`}
              />
              <SmallCard
                value={Math.min(...cluster.data.map((sensorData) => sensorData.value))}
                title={`Lowest ${DataType[cluster.dataType]}`}
              />
            </>
          ))}
        </div>
        {data?.map((cluster) => (
          <Card customClass="mb-5 h-md">
            <h2 className="pb-3 font-semibold self-start">{DataType[cluster.dataType]}</h2>
            <ResponsiveLineGraph
              dataType={cluster.dataType}
              data={cluster.data}
              limits={cluster.sensor.limits.filter(
                (limit) => limit.dataType === cluster.dataType
              )}></ResponsiveLineGraph>
          </Card>
        ))}
        {alertsQuery.data &&
          alertsQuery.data?.filter((alert) => alert.name === data![0].sensor.id || alert.name === data![0].sensor.name)
            .length > 0 && (
            <AlertSection
              data={alertsQuery.data?.filter(
                (alert) => alert.name === data![0].sensor.id || alert.name === data![0].sensor.name
              )}
              detailed={true}
            />
          )}
      </div>
    </Layout>
  );
}
