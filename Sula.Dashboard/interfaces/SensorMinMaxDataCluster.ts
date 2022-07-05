import { SensorMinMaxData } from '.';
import { DataType } from './DataType';
import { Sensor } from './Sensor';

export interface SensorMinMaxDataCluster {
  sensor: Sensor;
  data: {
    week: SensorMinMaxData[];
    month: SensorMinMaxData[];
    year: SensorMinMaxData[];
  };
  dataType: DataType;
}
