import { Component, DestroyRef, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';

import { Card } from '@components/cards';
import { Messages } from '@components/messages/messages';
import { MessagesService } from '@components/messages/messages.service';
import { ValidatorMessage } from '@components/validator-message/validator-message';

import { compareValidator } from '@app-types/validators';

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

  readonly #fb = inject(NonNullableFormBuilder);

  protected readonly form = this.#fb.group(
    {
      companyName: ['', [Validators.required]],
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', [Validators.required]],
    },
    {
      validators: [compareValidator('password', 'confirmPassword')],
    }
  );

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
