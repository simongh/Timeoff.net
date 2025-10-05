import { Component, DestroyRef, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { injectQueryParams } from 'ngxtension/inject-query-params';

import { Card } from '@components/cards';
import { Messages } from '@components/messages/messages';
import { MessagesService } from '@components/messages/messages.service';
import { ValidatorMessage } from '@components/validator-message/validator-message';

import { AuthService } from '@app-types/auth/auth.service';

import { LoginApi } from './login-api/login-api';

@Component({
  selector: 'ton-login-page',
  imports: [ReactiveFormsModule, Card, ValidatorMessage, RouterLink, Messages],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  readonly #loginSvc = inject(LoginApi);

  readonly #currentUserSvc = inject(AuthService);

  readonly #msgsSvc = inject(MessagesService);

  readonly #router = inject(Router);

  readonly #returnUrl = injectQueryParams((p) => p['returnUrl'] ?? '/');

  readonly #destroyed = inject(DestroyRef);

  protected readonly submitting = signal(false);

  protected readonly allowRegistrations = signal(true);

  protected readonly loginForm = this.#loginSvc.createLoginForm();

  protected login() {
    this.loginForm.markAllAsTouched();

    if (!this.loginForm.valid) {
      return;
    }

    this.submitting.set(true);

    this.#loginSvc
      .login(this.loginForm.value)
      .pipe(takeUntilDestroyed(this.#destroyed))
      .subscribe({
        next: (r) => {
          this.#currentUserSvc.clear();

          if (r.success) {
            this.#currentUserSvc.load(r);

            this.#router.navigateByUrl(this.#returnUrl());
          } else {
            this.clearPassword();

            this.#msgsSvc.addError('Unable to login');
          }

          this.submitting.set(false);
        },
        error: () => {
          this.clearPassword();
        },
      });
  }

  protected clearPassword() {
    this.loginForm.controls.password.setValue('');
    this.loginForm.markAsUntouched();
  }
}
