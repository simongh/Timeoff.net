import { dateString } from './dateString';

export interface CalendarDayModel {
  id: number;
  date: dateString;
  name: string;
  isHoliday: boolean;
}
