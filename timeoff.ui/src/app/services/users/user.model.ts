import { dateString } from '../../components/types';
import { TeamModel } from '../../models/team.model';

export interface UserModel {
    id: number;
    name: string;
    team: TeamModel;
    teamId: number;
    isAdmin: boolean;
    availableAllowance: number;
    daysUsed: number;
    isActive: boolean;
    startDate: dateString;
    endDate: dateString;
    firstName:  string;
    lastName: string;
    email: string;
    autoApprove: boolean
}
