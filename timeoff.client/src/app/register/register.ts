import { Component, DestroyRef, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

import { Card } from '@components/cards';
import { Messages } from '@components/messages/messages';
import { MessagesService } from '@components/messages/messages.service';
import { ValidatorMessage } from '@components/validator-message/validator-message';

import { RegisterApi } from './register-api/register-api';

export type RegisterForm = Register['form']['value'];

@Component({
  selector: 'ton-register',
  imports: [Card, Messages, ReactiveFormsModule, ValidatorMessage, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class Register {
  readonly #registerSvc = inject(RegisterApi);

  readonly #msgsSvc = inject(MessagesService);

  readonly #destroyed = inject(DestroyRef);

  protected readonly form = this.#registerSvc.createRegisterForm();

  protected readonly submitting = signal(false);

  public register() {
    this.form.markAllAsTouched();
    if (this.form.invalid) {
      return;
    }

    this.submitting.set(true);
    this.#registerSvc
      .register(this.form.value)
      .pipe(takeUntilDestroyed(this.#destroyed))
      .subscribe({
        next: () => {
          this.#msgsSvc.addSuccess(
            'Company registered successfully. Please login using the details you supplied'
          );
          this.form.reset();
        },
      });
  }
}
