import { Component, computed, inject } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

import { Card } from '@components/cards';
import { Messages } from '@components/messages/messages';
import { MessagesService } from '@components/messages/messages.service';
import { ValidatorMessage } from '@components/validator-message/validator-message';

import { ForgotPasswordApi } from './forgot-password-api/forgot-password-api';

@Component({
  selector: 'ton-forgot-password',
  imports: [Card, Messages, RouterLink, ReactiveFormsModule, ValidatorMessage],
  templateUrl: './forgot-password.html',
  styleUrl: './forgot-password.scss',
})
export class ForgotPassword {
  readonly #forgotPasswordSvc = inject(ForgotPasswordApi);

  readonly #msgsSvc = inject(MessagesService);

  protected readonly passwordForm = this.#forgotPasswordSvc.createPasswordForm();

  protected readonly submitting = computed(() =>
    this.#forgotPasswordSvc.forgotPassword.isLoading()
  );

  public forgot() {
    this.passwordForm.markAllAsTouched();
    if (this.passwordForm.invalid) {
      return;
    }

    this.#forgotPasswordSvc.forgotPassword.load({
      payload: [this.passwordForm.value.email!],
      subscriber: {
        next: () => {
          this.#msgsSvc.addSuccess(
            `Password reset email sent to ${this.passwordForm.controls.email.value}`
          );

          this.passwordForm.reset();
        },
      },
    });
  }
}
