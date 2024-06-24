import { Component, DestroyRef, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, Validators } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { injectQueryParams } from 'ngxtension/inject-query-params';

import { FlashComponent } from '@components/flash/flash.component';
import { ValidatorMessageComponent } from '@components/validator-message/validator-message.component';

import { AuthService } from '@services/auth/auth.service';
import { MessagesService } from '@services/messages/messages.service';

@Component({
    selector: 'reset-password-page',
    standalone: true,
    templateUrl: './reset-password.component.html',
    styleUrl: './reset-password.component.scss',
    imports: [FlashComponent, CommonModule, ReactiveFormsModule, ValidatorMessageComponent],
})
export class ResetPasswordComponent {
    protected get passwordForm() {
        return this.authSvc.resetForm;
    }

    protected readonly showCurrent = computed(() => this.authSvc.isUserLoggedIn());

    protected readonly submitting = signal(false);

    protected readonly token = injectQueryParams('t');

    constructor(
        private readonly authSvc: AuthService,
        private readonly msgsSvc: MessagesService,
        private destroyed: DestroyRef
    ) {
        this.passwordForm.controls.token.setValue(this.token());

        if (this.showCurrent()) {
            this.passwordForm.controls.current.addValidators(Validators.required);
        } else {
            if (!this.passwordForm.value.token) {
                this.msgsSvc.isError('Invalid reset link');
                this.submitting.set(true);
            }
        }
    }

    public save() {
        this.passwordForm.markAllAsTouched();

        if (this.passwordForm.invalid) {
            return;
        }

        this.submitting.set(true);

        this.authSvc
            .resetPassword()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.msgsSvc.isSuccess('Password updated successfully');
                    this.submitting.set(false);
                    this.passwordForm.reset();
                },
            });
    }
}
