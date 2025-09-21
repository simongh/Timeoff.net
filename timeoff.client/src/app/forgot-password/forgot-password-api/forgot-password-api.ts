import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ForgotPasswordApi {
  readonly #client = inject(HttpClient);

  public forgotPassword(email: string) {
    return this.#client.post<void>('/api/account/forgot-password', email);
  }
}
