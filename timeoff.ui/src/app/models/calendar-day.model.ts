import { LeaveStatus, datePart, dateString } from "./types";

export interface CalendarDayModel{
    id: number;
    name: string;
    date: dateString;
    isHoliday: boolean;
    colour: string | null;
    dayPart: datePart | null;
    status: LeaveStatus | null;
}