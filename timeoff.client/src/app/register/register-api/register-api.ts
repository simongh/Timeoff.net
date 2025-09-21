import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

import { RegisterForm } from '../register';

@Injectable({
  providedIn: 'root',
})
export class RegisterApi {

  readonly #client = inject(HttpClient);


  public register(form: RegisterForm) {
    return this.#client.post<void>('/api/account/register', form);
  }
}
