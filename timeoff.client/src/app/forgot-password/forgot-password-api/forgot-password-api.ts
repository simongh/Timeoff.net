import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { email, form, required } from '@angular/forms/signals';

import { injectApi } from '@app-types/apiResource';

@Injectable({
  providedIn: 'root',
})
export class ForgotPasswordApi {
  readonly #client = inject(HttpClient);

  public readonly forgotPassword = injectApi((email: string) =>
    this.#client.post<void>('/api/account/forgot-password', email)
  );

  public createPasswordForm() {
    const model = signal({
      email: ''
    })
    return form(model, (schema)=> {
      required(schema.email);
      email(schema.email);
    })
  }
}
