import { Injectable } from "@angular/core";
import { of, tap } from "rxjs";
import { ResetPasswordModel } from "./reset-password.model";
import { LoginModel } from "./login.model";

@Injectable({
    providedIn:'root'
})
export class AuthService{
    private isLoggedIn: boolean = false;

    public get isUserLoggedIn(){
        return this.isLoggedIn;
    }

    public login(model: LoginModel){
        return of(true)
            .pipe(tap(()=>this.isLoggedIn = true));
    }

    public logout(){
        this.isLoggedIn = false;
    }

    public resetPassword(model: ResetPasswordModel) {
        return of(true);
    }

    public forgotPassword(email: string) {
        return of(true);
    }
    
}