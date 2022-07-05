export function convertToFahrenheit(celsius: number): number {
  return (celsius * 9) / 5 + 32;
}

export function convertToDate(time: string | Date): string {
  const date = new Date(time);
  return `${date.getDate()}-${date.getMonth()}-${date.getFullYear()}`;
}
