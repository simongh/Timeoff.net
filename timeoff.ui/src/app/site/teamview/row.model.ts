import { DayModel } from "./day.model";

export interface RowModel{
    name: string;
    id: number;
    total: number;
    days: DayModel[];
    summary: string;
}