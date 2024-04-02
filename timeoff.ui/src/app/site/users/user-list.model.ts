import { TeamModel } from "@services/company/team.model";

export interface UserListModel {
    id: number;
    isActive: boolean;
    name: string;
    team: TeamModel;
    isAdmin: boolean;
    availableAllowance: number;
    daysUsed: number;
}