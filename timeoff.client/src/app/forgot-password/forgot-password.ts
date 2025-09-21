import { Component, DestroyRef, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
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

  readonly #destroyed = inject(DestroyRef);

  readonly #fb = inject(NonNullableFormBuilder);

  protected readonly passwordForm = this.#fb.group({
    email: ['', [Validators.required, Validators.email]],
  });

  protected readonly submitting = signal(false);

  public forgot() {
    this.passwordForm.markAllAsTouched();
    if (this.passwordForm.invalid) {
      return;
    }

    this.submitting.set(true);
    this.#forgotPasswordSvc
      .forgotPassword(this.passwordForm.value.email!)
      .pipe(takeUntilDestroyed(this.#destroyed))
      .subscribe({
        next: () => {
          this.#msgsSvc.addSuccess(
            `Password reset email sent to ${this.passwordForm.controls.email.value}`
          );

          this.passwordForm.reset();
          this.submitting.set(false);
        },
      });
  }
}
