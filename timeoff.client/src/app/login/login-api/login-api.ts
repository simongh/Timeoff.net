import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { NonNullableFormBuilder, Validators } from '@angular/forms';
import { of } from 'rxjs';

import { LoggedInUserModel } from '@api/auth/logged-in-user.model';

type LoginForm = ReturnType<LoginApi['createLoginForm']>['value'];

@Injectable({
  providedIn: 'root',
})
export class LoginApi {
  readonly #client = inject(HttpClient);

  readonly #fb = inject(NonNullableFormBuilder);

  public createLoginForm() {
    return this.#fb.group({
      username: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }

  public login(form: LoginForm) {
    return this.#client.post<LoginResult>('/api/account/login', form);
  }
}

interface LoginResult extends LoggedInUserModel {
  success: boolean;
  errors: string[] | null;
}
