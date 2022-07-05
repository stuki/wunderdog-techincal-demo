import { DataType } from './DataType';
import { Operator } from './Operator';

export interface Limit {
  hasAlertedRecently: boolean;
  id?: number;
  dataType: DataType;
  operator: Operator;
  value: number;
  isEnabled: boolean;
  alertTime?: Date;
  new?: boolean;
}
