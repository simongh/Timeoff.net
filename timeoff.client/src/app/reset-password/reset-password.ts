import { Component, computed, effect, inject, signal } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { injectQueryParams } from 'ngxtension/inject-query-params';

import { Card } from '@components/cards';
import { Messages } from '@components/messages/messages';
import { MessagesService } from '@components/messages/messages.service';
import { ValidatorMessage } from '@components/validator-message/validator-message';

import { AuthService } from '@app-types/auth/auth.service';

import { ResetPasswordApi } from './reset-password-api/reset-password-api';

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

  readonly #invalidLink = signal(false);

  protected readonly showCurrent = computed(() => this.#authSvc.isUserLoggedIn());

  protected readonly token = injectQueryParams('t');

  protected readonly resetForm = this.#resetPasswordSvc.createResetForm(
    this.showCurrent(),
    this.token()
  );

  protected readonly submitting = computed(
    () => this.#resetPasswordSvc.resetPassword.isLoading() || this.#invalidLink()
  );

  constructor() {
    effect(() => {
      if (!this.showCurrent()) {
        if (!this.resetForm.value.token) {
          this.#msgsSvc.addError('Invalid reset link');
          this.#invalidLink.set(true);
        }
      }
    });
  }

  public save() {
    this.resetForm.markAllAsTouched();

    if (this.resetForm.invalid) {
      return;
    }

    this.#resetPasswordSvc.resetPassword.load({
      payload: [this.resetForm.value],
      subscriber: {
        next: () => {
          this.#msgsSvc.addSuccess('Password updated successfully');
          this.resetForm.reset();
        },
      },
    });
  }
}
