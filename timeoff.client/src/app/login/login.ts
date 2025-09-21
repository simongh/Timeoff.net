import { Component, DestroyRef, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { NgbAlert } from '@ng-bootstrap/ng-bootstrap';
import { injectQueryParams } from 'ngxtension/inject-query-params';

import { Card } from '@components/cards';
import { ValidatorMessage } from '@components/validator-message/validator-message';

import { AuthApi } from '@api/auth/auth-api';
import { AuthService } from '@app-types/auth/auth.service';

@Component({
  selector: 'ton-login-page',
  imports: [ReactiveFormsModule, Card, ValidatorMessage, NgbAlert, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  readonly #authService = inject(AuthApi);

  readonly #currentUserSvc = inject(AuthService);

  readonly #router = inject(Router);

  readonly #returnUrl = injectQueryParams((p) => p['returnUrl'] ?? '/');

  readonly #destroyed = inject(DestroyRef);

  protected readonly submitting = signal(false);

  protected readonly error = signal('');

  protected readonly allowRegistrations = signal(true);

  protected get loginForm() {
    return this.#authService.loginForm;
  }

  public login() {
    this.loginForm.markAllAsTouched();

    if (!this.loginForm.valid) {
      return;
    }

    this.submitting.set(true);

    this.#authService
      .login()
      .pipe(takeUntilDestroyed(this.#destroyed))
      .subscribe({
        next: (r) => {
          this.#currentUserSvc.clear();

          if (r.success) {
            this.#currentUserSvc.load(r);

            this.#router.navigateByUrl(this.#returnUrl());
          } else {
            this.loginForm.controls.password.setValue('');
            this.loginForm.markAsUntouched();

            this.error.set('Unable to login');
          }

          this.submitting.set(false);
        },
        error:()=> {
          this.error.set('Unable to login');
        }
      });
  }
}
