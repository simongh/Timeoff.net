import { UserSummaryModel } from "./user-summary.model";
import { ScheduleModel } from "@models/schedule.model";

export interface TeamViewModel{
    users: UserSummaryModel[];
    schedule: ScheduleModel;
}