import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { NonNullableFormBuilder, Validators } from '@angular/forms';

@Injectable({
  providedIn: 'root',
})
export class ForgotPasswordApi {
  readonly #client = inject(HttpClient);

  readonly #fb = inject(NonNullableFormBuilder);

  public createPasswordForm() {
    return this.#fb.group({
      email: ['', [Validators.required, Validators.email]],
    });
  }

  public forgotPassword(email: string) {
    return this.#client.post<void>('/api/account/forgot-password', email);
  }
}
