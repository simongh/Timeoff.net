import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { email, form, required } from '@angular/forms/signals';

import { LoggedInUserModel } from '@api/auth/logged-in-user.model';
import { injectApi } from '@app-types/apiResource';

interface LoginModel {
  username: string;
  password: string;
}

@Injectable({
  providedIn: 'root',
})
export class LoginApi {
  readonly #client = inject(HttpClient);

  public readonly login = injectApi((form: LoginModel) =>
    this.#client.post<LoginResult>('/api/account/login', form),
  );

  public createLoginForm() {
    const model = signal<LoginModel>({
      username: '',
      password: '',
    });

    return form(model, (schema) => {
      required(schema.username);
      email(schema.username);
      
      required(schema.password);
    });
  }
}

interface LoginResult extends LoggedInUserModel {
  success: boolean;
  errors: string[] | null;
}
