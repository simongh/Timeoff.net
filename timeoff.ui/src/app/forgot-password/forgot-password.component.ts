import { Component, DestroyRef, inject, signal } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

import { FlashComponent } from '@components/flash/flash.component';
import { ValidatorMessageComponent } from '@components/validator-message/validator-message.component';

import { AuthService } from '@services/auth/auth.service';
import { MessagesService } from '@services/messages/messages.service';

@Component({
    selector: 'forgot-password-page',
    templateUrl: 'forgot-password.component.html',
    providers: [AuthService],
    imports: [FlashComponent, ReactiveFormsModule, ValidatorMessageComponent],
})
export class ForgotPasswordComponent {
    readonly #passwordSvc = inject(AuthService);

    readonly #msgsSvc = inject(MessagesService);

    readonly #destroyed = inject(DestroyRef);

    protected get passwordForm() {
        return this.#passwordSvc.passwordForm;
    }

    protected readonly submitting = signal(false);

    public forgot() {
        this.passwordForm.markAllAsTouched();
        if (this.passwordForm.invalid) return;

        this.submitting.set(true);
        this.#passwordSvc
            .forgotPassword()
            .pipe(takeUntilDestroyed(this.#destroyed))
            .subscribe({
                next: () => {
                    this.#msgsSvc.isSuccess(`Password reset email sent to ${this.passwordForm.controls.email.value}`);

                    this.passwordForm.reset();
                    this.submitting.set(false);
                },
            });
    }
}