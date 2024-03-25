import { datePart } from '@components/types';
import { UserModel } from '@services/company/user.model';

export interface LeaveRequestModel {
    id: number;
    startDate: Date;
    startPart: datePart;
    endDate: Date;
    endPart: datePart;
    days: number;
    approver: UserModel;
    type: string;
    comment: string;
    status: string;
}
