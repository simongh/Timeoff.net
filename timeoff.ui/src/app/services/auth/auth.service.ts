import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormBuilder, Validators } from '@angular/forms';
import { catchError, of, tap } from 'rxjs';

import { compareValidator } from '@components/validators';

import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';
import { LoggedInUserModel } from '@models/logged-in-user.model';

interface LoginResult extends LoggedInUserModel {
    success: boolean;
    errors: string[] | null;
}

@Injectable({
    providedIn: 'root',
})
export class AuthService {
    public get isUserLoggedIn() {
        return this.loggedInUser.isUserLoggedIn;
    }

    public loginForm = this.fb.group({
        username: ['', [Validators.required, Validators.email]],
        password: ['', Validators.required],
    });

    public passwordForm = this.fb.group({
        email: ['', [Validators.required, Validators.email]],
    });

    public resetForm = this.fb.group(
        {
            current: [''],
            password: ['', [Validators.required, Validators.minLength(8)]],
            confirmPassword: ['', []],
            token: [null as string | null]
        },
        {
            validators: [compareValidator('password', 'confirmPassword')],
        }
    );

    constructor(
        private client: HttpClient,
        private readonly loggedInUser: LoggedInUserService,
        private readonly fb: FormBuilder
    ) {}

    public getToken() {
        return of('token').pipe(
            catchError((e) => {
                this.loggedInUser.clear();
                throw e;
            }),
        );
    }

    public login() {
        return this.client.post<LoginResult>('/api/account/login', this.loginForm.value);
    }

    public logout() {
        return this.client.post<void>('/api/account/logout', {});
    }

    public resetPassword() {
        return this.client.post<void>('/api/account/reset-password', this.resetForm.value);
    }

    public forgotPassword() {
        return this.client.post<void>('/api/account/forgot-password', this.passwordForm.value);
    }
}
