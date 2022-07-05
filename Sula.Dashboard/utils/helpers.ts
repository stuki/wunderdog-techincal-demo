import { DataType, SensorData, SensorMinMaxData } from '../interfaces';

export function getDataType(dataType: DataType) {
  switch (dataType) {
    case DataType.Humidity:
      return 'Humidity';
    case DataType.Temperature:
      return 'Temperature';
  }
}

export function orderByTimeAscending(a: SensorData | SensorMinMaxData, b: SensorData | SensorMinMaxData): number {
  return new Date(a.time).getTime() - new Date(b.time).getTime();
}

export function orderByTimeDescending(a: SensorData | SensorMinMaxData, b: SensorData | SensorMinMaxData): number {
  return new Date(b.time).getTime() - new Date(a.time).getTime();
}
