import { Sensor, Settings } from '.';

export interface User {
  settings: Settings;
  email: string;
  phoneNumber?: string | null;
  sensors: Sensor[];
}
