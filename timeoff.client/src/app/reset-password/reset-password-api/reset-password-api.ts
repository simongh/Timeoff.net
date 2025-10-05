import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { NonNullableFormBuilder, Validators } from '@angular/forms';

import { compareValidator } from '@app-types/validators';

type ResetForm = ReturnType<ResetPasswordApi['createResetForm']>['value'];

@Injectable({
  providedIn: 'root',
})
export class ResetPasswordApi {
  readonly #client = inject(HttpClient);

  readonly #fb = inject(NonNullableFormBuilder);

  public createResetForm(showCurrent: boolean, token: string | null) {
    return this.#fb.group(
      {
        current: ['', showCurrent ? [Validators.required] : []],
        password: ['', [Validators.required, Validators.minLength(8)]],
        confirmPassword: ['', []],
        token: [token],
      },
      {
        validators: [compareValidator('password', 'confirmPassword')],
      }
    );
  }

  public resetPassword(form: ResetForm) {
    return this.#client.post<void>('/api/account/reset-password', form);
  }
}
