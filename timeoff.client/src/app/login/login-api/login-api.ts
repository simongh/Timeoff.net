import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { of } from 'rxjs';

import { LoggedInUserModel } from '@api/auth/logged-in-user.model';

import { LoginForm } from '../login';

@Injectable({
  providedIn: 'root',
})
export class LoginApi {
  readonly #client = inject(HttpClient);

  public login(form: LoginForm) {
    //return this.#client.post<LoginResult>('/api/account/login', form);
    return of<LoginResult>({
      success: true,
      errors: null,
      isAdmin: true,
      showTeamView: true,
      companyName: 'testco',
      name: 'test user',
      token: 'token',
      dateFormat: 'yyyy-M-d',
      expires: '2025-09-21T19:40'
    })
  }

}

interface LoginResult extends LoggedInUserModel {
  success: boolean;
  errors: string[] | null;
}

