import { dateString } from "./types";

export interface PublicHolidayModel {
    id: number | null;
    date: dateString | null;
    name: string;
}
