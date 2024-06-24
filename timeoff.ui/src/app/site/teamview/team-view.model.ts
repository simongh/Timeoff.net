import { ScheduleModel } from "@models/schedule.model";

import { UserSummaryModel } from "./user-summary.model";

export interface TeamViewModel{
    users: UserSummaryModel[];
    schedule: ScheduleModel;
}