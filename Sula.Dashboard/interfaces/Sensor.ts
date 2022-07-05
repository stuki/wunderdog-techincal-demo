import { Limit } from './Limit';
import { SensorType } from './SensorType';

export interface Sensor {
  id: string;
  name: string;
  sensorType: SensorType;
  limits: Limit[];
}
