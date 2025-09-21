import { dateString } from '@app-types/dateString';

export interface LoggedInUserModel {
    isAdmin: boolean;
    showTeamView: boolean;
    dateFormat: string | null;
    companyName: string | null;
    name: string | null;
    token: string | null;
    expires: dateString | null;
}