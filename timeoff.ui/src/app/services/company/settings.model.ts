import { ScheduleModel } from '@models/schedule.model';
import { LeaveTypeModel } from './leave-type.model';

export interface SettingsModel {
    name: string;
    country: string;
    dateFormat: string;
    timeZone: string;
    carryOver: number;
    showHoliday: boolean;
    hideTeamView: boolean;
    leaveTypes: LeaveTypeModel[];
    schedule: ScheduleModel;
}
