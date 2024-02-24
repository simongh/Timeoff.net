import { Injectable } from "@angular/core";
import { catchError, map, of, tap } from "rxjs";
import { ResetPasswordModel } from "./reset-password.model";
import { LoginModel } from "./login.model";
import { HttpClient, HttpErrorResponse } from "@angular/common/http";

interface LoginResult {
    token: string,
    success: boolean,
    expires: Date,
    errors: string[] | null,
}

@Injectable({
    providedIn:'root'
})
export class AuthService{
    private token: string | null = null;

    public get isUserLoggedIn(){
        return !!this.token;
    }

    constructor(
        private client: HttpClient) {}

    public getToken() {
        return of("token")
            .pipe(
                catchError((e)=>{
                    this.token = null;
                    throw e;
                }),
                tap(() => this.token = 'token'));
    }

    public login(model: LoginModel) {
        return this.client.post<LoginResult>('/api/account/login',{
            username: model.email,
            password: model.password
        }).pipe(
            catchError((err: HttpErrorResponse) => {
                if (err.status === 400) {
                    return of({success: false, errors: ['Invalid credentials']})
                } else {
                    return of({success: false, errors: ['Unable to login. Please try again later']} as LoginResult) 
                }
            }),
            tap((r) => this.token = 'token'),
            map((r) => r.errors)
        );
    }

    public logout() {
        return this.client.post<void>('/api/account/logout',{})
            .pipe(tap(() => this.token = null));
    }

    public resetPassword(model: ResetPasswordModel) {
        return this.client.post<void>('/api/account/reset-password',model);
    }

    public forgotPassword(email: string) {
        return this.client.post<void>('/api/account/forgot-password',{ email: email })
    }
}