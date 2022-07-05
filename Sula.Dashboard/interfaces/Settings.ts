import { TemperatureUnit } from '.';

export enum Language {
  en,
  fi,
}

export interface Settings {
  language: Language;
  temperature: TemperatureUnit;
}
