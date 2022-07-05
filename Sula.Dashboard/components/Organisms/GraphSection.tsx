import Link from 'next/link';
import React, { useEffect, useState } from 'react';
import { DataType, SensorMinMaxDataCluster } from '../../interfaces';
import { orderByTimeDescending } from '../../utils';
import { ResponsiveBoxGraph } from '../Atoms';
import { Card } from '../Molecules';
import { Period } from '../../interfaces';
import useTranslation from 'next-translate/useTranslation';

export function GraphSection({ cluster }: { cluster?: SensorMinMaxDataCluster[] }) {
  if (!cluster) {
    return null;
  }

  return (
    <>
      {cluster
        .sort((a, b) => a.sensor.id.localeCompare(b.sensor.id))
        .map((cluster) => (
          <GraphCard cluster={cluster} />
        ))}
    </>
  );
}

function GraphCard({ cluster }: { cluster: SensorMinMaxDataCluster }) {
  const { t } = useTranslation('graph');
  const [selectedPeriod, setSelectedPeriod] = useState(Period.Week);
  const [data, setData] = useState(cluster.data.week.sort(orderByTimeDescending));

  const sensorName = cluster.sensor.name ?? cluster.sensor.id;

  useEffect(() => {
    switch (selectedPeriod) {
      case Period.Week:
        setData(cluster.data.week.sort(orderByTimeDescending));
        break;
      case Period.Month:
        setData(cluster.data.month.sort(orderByTimeDescending));
        break;
      case Period.Year:
        setData(cluster.data.year.sort(orderByTimeDescending));
        break;
    }
  }, [selectedPeriod]);

  return (
    <Card customClass="h-md md:col-span-6">
      <div className="flex w-full justify-between px-2 pt-4">
        <Link href={`/sensors/${cluster.sensor.id}`}>
          <a className="hover:opacity-80 font-semibold">{`${sensorName} : ${t(DataType[cluster.dataType])}`}</a>
        </Link>
        <div className="flex justify-end">
          <PeriodButton period={Period.Week} selectedPeriod={selectedPeriod} setSelectedPeriod={setSelectedPeriod} />
          <PeriodButton period={Period.Month} selectedPeriod={selectedPeriod} setSelectedPeriod={setSelectedPeriod} />
          <PeriodButton period={Period.Year} selectedPeriod={selectedPeriod} setSelectedPeriod={setSelectedPeriod} />
        </div>
      </div>
      <div className="h-full w-full px-2">
        <ResponsiveBoxGraph data={data} period={selectedPeriod}></ResponsiveBoxGraph>
      </div>
    </Card>
  );
}

function PeriodButton({
  period,
  selectedPeriod,
  setSelectedPeriod,
}: {
  period: Period;
  selectedPeriod: Period;
  setSelectedPeriod: React.Dispatch<React.SetStateAction<Period>>;
}) {
  const { t } = useTranslation('graph');
  const selected = selectedPeriod === period ? ' color-primary-lighter text-black' : '';

  return (
    <button
      onClick={() => setSelectedPeriod(period)}
      className={'rounded-md md:p-3 p-2 text-xs focus:outline-none hover:opacity-80 ' + selected}>
      {t(Period[period])}
    </button>
  );
}
