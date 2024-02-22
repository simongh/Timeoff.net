export interface ResetPasswordModel{
    password: string | null;
    newPassword: string;
    confirmPassword: string;
    token: string | null;
}