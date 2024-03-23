import { dateString } from '../../components/types';
import { ScheduleModel } from '../../models/schedule.model';
import { TeamModel } from '../company/team.model';

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
    firstName: string;
    lastName: string;
    email: string;
    autoApprove: boolean;
    schedule: ScheduleModel;
    scheduleOverride: boolean;
}
