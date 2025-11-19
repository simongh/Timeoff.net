import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { NonNullableFormBuilder, Validators } from '@angular/forms';
import { of } from 'rxjs';

import { LoggedInUserModel } from '@api/auth/logged-in-user.model';
import { injectApi } from '@app-types/apiResource';

type LoginForm = ReturnType<LoginApi['createLoginForm']>['value'];

@Injectable({
  providedIn: 'root',
})
export class LoginApi {
  readonly #client = inject(HttpClient);

  readonly #fb = inject(NonNullableFormBuilder);

  public readonly login = injectApi((form: LoginForm) =>
    this.#client.post<LoginResult>('/api/account/login', form)
  );

  public createLoginForm() {
    return this.#fb.group({
      username: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }
}

interface LoginResult extends LoggedInUserModel {
  success: boolean;
  errors: string[] | null;
}
