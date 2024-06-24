import { dateString } from '@models/types';
import { ScheduleModel } from '@models/schedule.model';

export interface UserModel {
    id: number;
    name: string;
    team: number;
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
