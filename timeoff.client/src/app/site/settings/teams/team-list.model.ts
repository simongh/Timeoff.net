import { UserModel } from "@app-types/user.model";

export interface TeamListModel {
    id: number;
    name: string;
    manager: UserModel;
    allowance: number;
    employeeCount: number;
    includePublicHolidays: boolean;
    isAccruedAllowance: boolean;
}