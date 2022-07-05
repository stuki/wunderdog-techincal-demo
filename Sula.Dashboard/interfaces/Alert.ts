import { DataType, Operator } from '.';

export interface Alert {
  name: string;
  alertTime: Date;
  latitude: number;
  longitude: number;
  dataType: DataType;
  operator: Operator;
  value: number;
}
