import { LeaveStatus, datePart } from "./types";

export interface CalendarDayModel{
    id: number;
    name: string;
    date: Date;
    isHoliday: boolean;
    colour: string | null;
    dayPart: datePart | null;
    status: LeaveStatus | null;
}