import { CalendarDayModel } from "@models/calendar-day.model";
import { ScheduleModel } from "@models/schedule.model";
import { UserModel } from "@services/company/user.model";

export interface UserSummaryModel{
    id: number;
    name: string;
    user: UserModel;
    schedule: ScheduleModel;
    used: number;
    days: CalendarDayModel[];
}
