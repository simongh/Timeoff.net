import { PublicHolidayModel } from "@models/public-holiday.model";
import { UserSummaryModel } from "./user-summary.model";
import { ScheduleModel } from "@models/schedule.model";

export interface TeamViewModel{
    holidays: PublicHolidayModel[];
    users: UserSummaryModel[];
    schedule: ScheduleModel;
}