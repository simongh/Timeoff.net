export interface ResetPasswordModel{
    current: string | null;
    password: string;
    token: string | null;
}