import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { NonNullableFormBuilder, Validators } from '@angular/forms';

import { injectApi } from '@app-types/apiResource';

@Injectable({
  providedIn: 'root',
})
export class ForgotPasswordApi {
  readonly #client = inject(HttpClient);

  readonly #fb = inject(NonNullableFormBuilder);

  public readonly forgotPassword = injectApi((email: string) =>
    this.#client.post<void>('/api/account/forgot-password', email)
  );

  public createPasswordForm() {
    return this.#fb.group({
      email: ['', [Validators.required, Validators.email]],
    });
  }
}
