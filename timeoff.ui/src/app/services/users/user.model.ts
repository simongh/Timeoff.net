import { TeamModel } from "../../models/team.model";

export interface UserModel {
    id: number;
    name: string;
    team: TeamModel;
    isAdmin: boolean;
    availableAllowance: number;
    daysUsed: number;
    isActive: boolean;
}