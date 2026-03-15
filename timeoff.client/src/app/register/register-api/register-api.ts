import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { email, form, minLength, required, validate } from '@angular/forms/signals';

import { injectApi } from '@app-types/apiResource';
import { compare } from '@app-types/validators';

interface RegisterModel {
  companyName: string;
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  confirmPassword: string;
}

@Injectable({
  providedIn: 'root',
})
export class RegisterApi {
  readonly #client = inject(HttpClient);

  public readonly register = injectApi((form: RegisterModel) =>
    this.#client.post<void>('/api/account/register', form),
  );

  public createRegisterForm() {
    const model = signal<RegisterModel>({
      companyName: '',
      firstName: '',
      lastName: '',
      email: '',
      password: '',
      confirmPassword: '',
    });

    return form(model, (schema) => {
      required(schema.companyName);
      required(schema.firstName);
      required(schema.lastName);

      required(schema.email);
      email(schema.email);

      required(schema.password);
      minLength(schema.password, 8);

      required(schema.confirmPassword);

      compare(schema.confirmPassword, schema.password);
    });
  }
}
