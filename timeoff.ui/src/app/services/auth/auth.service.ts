import { Injectable } from "@angular/core";
import { catchError, map, of, tap } from "rxjs";
import { ResetPasswordModel } from "./reset-password.model";
import { LoginModel } from "./login.model";
import { HttpClient, HttpErrorResponse } from "@angular/common/http";

interface loginResult {
    token: string,
    success: boolean,
    expires: Date,
    errors: string[] | null,
}

@Injectable({
    providedIn:'root'
})
export class AuthService{
    private isLoggedIn: boolean = false;

    public get isUserLoggedIn(){
        return this.isLoggedIn;
    }

    constructor(
        private client: HttpClient) {}

    public login(model: LoginModel) {
        return this.client.post<loginResult>('/api/account/login',{
            username: model.email,
            password: model.password
        }).pipe(
            catchError((err: HttpErrorResponse) => {
                if (err.status === 400) {
                    return of({success: false, errors: ['Invalid credentials']})
                } else {
                    return of({success: false, errors: ['Unable to login. Please try again later']} as loginResult) 
                }
            }),
            tap((r) => this.isLoggedIn = r.success),
            map((r) => r.errors)
        );
    }

    public logout() {
        return this.client.post<void>('/api/account/logout',{})
            .pipe(tap(() => this.isLoggedIn = false));
    }

    public resetPassword(model: ResetPasswordModel) {
        return this.client.post<void>('/api/account/reset-password',model);
    }

    public forgotPassword(email: string) {
        return this.client.post<void>('/api/account/forgot-password',{ email: email })
    }
}