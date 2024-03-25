import { UserModel } from '@services/company/user.model';

export interface EmailModel {
    user: UserModel;
    subject: string;
    email: string;
    createdAt: Date;
    body: string;
}
