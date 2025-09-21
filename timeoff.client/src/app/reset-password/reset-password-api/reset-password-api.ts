import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

import { ResetForm } from '../reset-password';

@Injectable({
  providedIn: 'root',
})
export class ResetPasswordApi {
  readonly #client = inject(HttpClient);

  public resetPassword(form: ResetForm) {
    return this.#client.post<void>('/api/account/reset-password', form);
  }
}
