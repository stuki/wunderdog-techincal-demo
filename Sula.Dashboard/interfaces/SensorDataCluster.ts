import { DataType } from './DataType';
import { Sensor } from './Sensor';
import { SensorData } from './SensorData';

export interface SensorDataCluster {
  sensor: Sensor;
  data: SensorData[];
  dataType: DataType;
  latestEntry: SensorData;
}
