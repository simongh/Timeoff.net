import { Component, computed, DestroyRef, effect, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { injectQueryParams } from 'ngxtension/inject-query-params';

import { Card } from '@components/cards';
import { Messages } from '@components/messages/messages';
import { MessagesService } from '@components/messages/messages.service';
import { ValidatorMessage } from '@components/validator-message/validator-message';

import { AuthService } from '@app-types/auth/auth.service';
import { compareValidator } from '@app-types/validators';

import { ResetPasswordApi } from './reset-password-api/reset-password-api';

export type ResetForm = ResetPassword['resetForm']['value'];

@Component({
  selector: 'ton-reset-password',
  imports: [ValidatorMessage, Messages, Card, ReactiveFormsModule, RouterLink],
  templateUrl: './reset-password.html',
  styleUrl: './reset-password.scss',
})
export class ResetPassword {
  readonly #resetPasswordSvc = inject(ResetPasswordApi);

  readonly #msgsSvc = inject(MessagesService);

  readonly #authSvc = inject(AuthService);

  readonly #destroyed = inject(DestroyRef);

  readonly #fb = inject(NonNullableFormBuilder);

  protected readonly showCurrent = computed(() => this.#authSvc.isUserLoggedIn());

  protected readonly token = injectQueryParams('t');

  protected readonly resetForm = this.#fb.group(
    {
      current: ['', this.showCurrent() ? [Validators.required] : []],
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', []],
      token: [this.token() as string | null],
    },
    {
      validators: [compareValidator('password', 'confirmPassword')],
    }
  );

  protected readonly submitting = signal(false);

  constructor() {
    effect(() => {
      if (!this.showCurrent()) {
        if (!this.resetForm.value.token) {
          this.#msgsSvc.addError('Invalid reset link');
          this.submitting.set(true);
        }
      }
    });
  }

  public save() {
    this.resetForm.markAllAsTouched();

    if (this.resetForm.invalid) {
      return;
    }

    this.submitting.set(true);

    this.#resetPasswordSvc
      .resetPassword(this.resetForm.value)
      .pipe(takeUntilDestroyed(this.#destroyed))
      .subscribe({
        next: () => {
          this.#msgsSvc.addSuccess('Password updated successfully');
          this.submitting.set(false);
          this.resetForm.reset();
        },
      });
  }
}
