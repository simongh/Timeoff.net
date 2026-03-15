import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { disabled, form, minLength, required } from '@angular/forms/signals';

import { injectApi } from '@app-types/apiResource';
import { compare } from '@app-types/validators';

interface ResetModel {
  current: string;
  password: string;
  confirmPassword: string;
  token: string | null;
}

@Injectable({
  providedIn: 'root',
})
export class ResetPasswordApi {
  readonly #client = inject(HttpClient);

  public readonly resetPassword = injectApi((form: ResetModel) =>
    this.#client.post<void>('/api/account/reset-password', form),
  );

  public createResetForm(showCurrent: boolean, token: string | null) {
    const model = signal<ResetModel>({
      password: '',
      confirmPassword: '',
      token: null,
      current: '',
    });

    return form(model, (schema) => {
      required(schema.current, { when: () => showCurrent });
      disabled(schema.current, () => !showCurrent);

      required(schema.password);
      minLength(schema.password, 8);

      required(schema.confirmPassword);
      compare(schema.confirmPassword, schema.password);
    });
  }
}
