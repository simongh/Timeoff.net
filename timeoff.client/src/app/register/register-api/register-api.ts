import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { NonNullableFormBuilder, Validators } from '@angular/forms';

import { injectApi } from '@app-types/apiResource';
import { compareValidator } from '@app-types/validators';

type RegisterForm = ReturnType<RegisterApi['createRegisterForm']>['value'];

@Injectable({
  providedIn: 'root',
})
export class RegisterApi {
  readonly #client = inject(HttpClient);

  readonly #fb = inject(NonNullableFormBuilder);

  public readonly register = injectApi((form: RegisterForm) => this.#client.post<void>('/api/account/register', form));

  public createRegisterForm() {
    return this.#fb.group(
      {
        companyName: ['', [Validators.required]],
        firstName: ['', [Validators.required]],
        lastName: ['', [Validators.required]],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(8)]],
        confirmPassword: ['', [Validators.required]],
      },
      {
        validators: [compareValidator('password', 'confirmPassword')],
      }
    );
  }
}
