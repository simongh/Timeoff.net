import { TeamModel } from "@services/company/team.model";
import { UserModel } from "@services/company/user.model";

export interface LeaveResultModel{
    user: UserModel;
    manager: UserModel;
    team: TeamModel
    start: Date;
    end: Date;
    days: number;
    added: Date;
    status: number;
    comment: string;
}