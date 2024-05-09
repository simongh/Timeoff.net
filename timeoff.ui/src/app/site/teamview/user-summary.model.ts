import { CalendarDayModel } from "@models/calendar-day.model";
import { ScheduleModel } from "@models/schedule.model";
import { TeamModel } from "@services/company/team.model";

export interface UserSummaryModel{
    id: number;
    name: string;
    schedule: ScheduleModel;
    total: number;
    team: TeamModel;
    days: CalendarDayModel[];
}