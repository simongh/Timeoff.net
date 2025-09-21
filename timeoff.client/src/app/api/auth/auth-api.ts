import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

import { compareValidator } from '@app-types/validators';

import { LoggedInUserModel } from './logged-in-user.model';

@Injectable({
  providedIn: 'root',
})
export class AuthApi {
  readonly #client = inject(HttpClient);

  readonly #fb = inject(FormBuilder);

  public loginForm = this.#fb.group({
    username: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required],
  });

    public passwordForm = this.#fb.group({
        email: ['', [Validators.required, Validators.email]],
    });

    public resetForm = this.#fb.group(
        {
            current: [''],
            password: ['', [Validators.required, Validators.minLength(8)]],
            confirmPassword: ['', []],
            token: [null as string | null],
        },
        {
            validators: [compareValidator('password', 'confirmPassword')],
        }
    );

    public login() {
    return this.#client.post<LoginResult>('/api/account/login', this.loginForm.value);
  }

  public logout() {
    return this.#client.post<void>('/api/account/logout', {});
  }

  public resetPassword() {
    return this.#client.post<void>('/api/account/reset-password', this.resetForm.value);
  }

  public forgotPassword() {
    return this.#client.post<void>('/api/account/forgot-password', this.passwordForm.value);
  }
}

interface LoginResult extends LoggedInUserModel {
  success: boolean;
  errors: string[] | null;
}
