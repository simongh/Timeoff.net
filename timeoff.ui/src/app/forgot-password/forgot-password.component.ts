import { Component, DestroyRef, signal } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

import { FlashComponent } from '@components/flash/flash.component';
import { ValidatorMessageComponent } from '@components/validator-message/validator-message.component';

import { AuthService } from '@services/auth/auth.service';
import { MessagesService } from '@services/messages/messages.service';

@Component({
    selector: 'forgot-password-page',
    standalone: true,
    templateUrl: 'forgot-password.component.html',
    providers: [AuthService],
    imports: [FlashComponent, ReactiveFormsModule, NgIf, ValidatorMessageComponent],
})
export class ForgotPasswordComponent {
    protected get passwordForm() {
        return this.passwordSvc.passwordForm;
    }

    protected readonly submitting = signal(false);

    constructor(
        private passwordSvc: AuthService,
        private msgsSvc: MessagesService,
        private destroyed: DestroyRef
    ) {}

    public forgot() {
        this.passwordForm.markAllAsTouched();
        if (this.passwordForm.invalid) return;

        this.submitting.set(true);
        this.passwordSvc
            .forgotPassword()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.msgsSvc.isSuccess(`Password reset email sent to ${this.passwordForm.controls.email.value}`);

                    this.passwordForm.reset();
                    this.submitting.set(false);
                },
                error: () => {
                    this.msgsSvc.isError('Unable to send reset email. Please try again later');
                    this.submitting.set(false);
                },
            });
    }
}
