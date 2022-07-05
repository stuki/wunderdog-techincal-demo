import React, { ReactElement } from 'react';
import { Sensor, SensorType } from '../../interfaces';
import { Card } from '../Molecules';

export function SensorsSection({ sensors }: { sensors: Sensor[] }): ReactElement {
  return (
    <div className="grid md:grid-cols-2 gap-10">
      {sensors.map((sensor: Sensor) => (
        <SensorCard sensor={sensor} />
      ))}
    </div>
  );
}

function SensorCard({ sensor }: { sensor: Sensor }) {
  return (
    <Card customClass="flex-row">
      <div className="bg-red-900 w-40 h-full"></div>
      <div className="p-5">
        <h2 className="text-lg">{sensor.name ?? sensor.id}</h2>
        <h4 className="text-sm">Type: {SensorType[sensor.sensorType]}</h4>
        <span className="text-sm">{sensor.limits.some((limit) => limit.hasAlertedRecently) ? 'yay' : 'nay'}</span>
        {/* {JSON.stringify(sensor)} */}
      </div>
    </Card>
  );
}
