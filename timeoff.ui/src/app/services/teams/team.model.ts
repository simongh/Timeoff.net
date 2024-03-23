import { UserModel } from '../company/user.model';

export interface TeamModel {
    id: number;
    name: string;
    manager: UserModel;
    allowance: number;
    employeeCount: number;
    includePublicHolidays: boolean;
    isAccruedAllowance: boolean;
}
