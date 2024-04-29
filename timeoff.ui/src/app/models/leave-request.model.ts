import { datePart } from '@models/types';
import { UserModel } from '@services/company/user.model';

export interface LeaveRequestModel {
    id: number;
    startDate: Date;
    startPart: datePart;
    endDate: Date;
    endPart: datePart;
    days: number;
    approver: UserModel;
    type: LeaveTypeModel;
    comment: string;
    status: string;
}

interface LeaveTypeModel {
    name: string;
    colour: string;
}
