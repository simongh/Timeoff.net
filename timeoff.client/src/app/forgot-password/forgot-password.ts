import { Component, computed, inject } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { FormField } from '@angular/forms/signals';
import { RouterLink } from '@angular/router';

import { Card } from '@components/cards';
import { Messages } from '@components/messages/messages';
import { MessagesService } from '@components/messages/messages.service';
import { ValidatorMessage } from '@components/validator-message/validator-message';

import { ForgotPasswordApi } from './forgot-password-api/forgot-password-api';

@Component({
  selector: 'ton-forgot-password',
  imports: [Card, Messages, RouterLink, ReactiveFormsModule, ValidatorMessage, FormField],
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
    this.passwordForm().reset();
    if (this.passwordForm().invalid()) {
      return;
    }

    this.#forgotPasswordSvc.forgotPassword.load({
      payload: [this.passwordForm.email().value()],
      subscriber: {
        next: () => {
          this.#msgsSvc.addSuccess(
            `Password reset email sent to ${this.passwordForm.email().value()}`
          );

          this.passwordForm().reset();
        },
      },
    });
  }
}
