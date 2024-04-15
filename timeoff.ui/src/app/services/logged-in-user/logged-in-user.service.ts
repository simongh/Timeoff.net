import { Injectable } from '@angular/core';
import { LoggedInUserModel } from '@models/logged-in-user.model';

@Injectable({
    providedIn: 'root',
})
export class LoggedInUserService {
    public get companyName(): string {
        return sessionStorage.getItem('companyName') || '';
    }

    public get userName(): string {
        return sessionStorage.getItem('userName') || '';
    }

    public get showTeamView(): boolean {
        return !!sessionStorage.getItem('showTeamView');
    }

    public get isAdmin(): boolean {
        return !!sessionStorage.getItem('isAdmin');
    }

    public get token(): string | null {
        return sessionStorage.getItem('token');
    }

    public get expires(): Date | null {
        const value = sessionStorage.getItem('expires');
        return !!value ? new Date(value) : null;
    }

    public get dateFormat(): string {
        const value = sessionStorage.getItem('dateFormat');
        return value || 'yyyy-MM-dd';
    }

    public get isUserLoggedIn() {
        return !!this.token;
    }

    constructor() {}

    public load(user: LoggedInUserModel) {
        sessionStorage.setItem('companyName',user.companyName!);
        sessionStorage.setItem('userName',user.name!);

        if (user.showTeamView) {
            sessionStorage.setItem('showTeamView', 'true');
        } else {
            sessionStorage.removeItem('showTeamView');
        }

        if (user.isAdmin) {
            sessionStorage.setItem('isAdmin', 'true');
        } else {
            sessionStorage.removeItem('isAdmin');
        }

        sessionStorage.setItem('token', user.token!);
        sessionStorage.setItem('expires', user.expires!.toString());
    }

    public clear() {
        sessionStorage.clear();
    }

    public extend(token: string, expires: Date) {
        sessionStorage.setItem('token',token);
        sessionStorage.setItem('expires',expires.toString());
    }
}
