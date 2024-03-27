import { ScheduleModel } from "@models/schedule.model";
import { LeaveSummary } from "@services/calendar/leave-summary.model";
import { TeamModel } from "@services/company/team.model";

export interface UserSummaryModel{
    id: number;
    name: string;
    leaveSummary: LeaveSummary[];
    schedule: ScheduleModel;
    total: number;
    team: TeamModel;
}