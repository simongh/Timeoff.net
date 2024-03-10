import { Component, DestroyRef, inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { FlashComponent } from "../components/flash/flash.component";
import { NgIf } from "@angular/common";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { AuthService } from "../services/auth/auth.service";
import { ValidatorMessageComponent } from "../components/validator-message/validator-message.component";
import { FlashModel, isError, isSuccess } from "../components/flash/flash.model";

@Component({
    standalone: true,
    templateUrl: 'forgot-password.component.html',
    providers: [AuthService],
    imports: [FlashComponent, ReactiveFormsModule, NgIf, ValidatorMessageComponent]
})
export class ForgotPasswordComponent {
    public passwordForm = this.fb.group({
        email: ['', [Validators.required, Validators.email]]
    })

    public messages = new FlashModel();

    public submitting = false;

    constructor(
        private fb: FormBuilder,
        private passwordSvc: AuthService,
        private destroyed: DestroyRef)
    {}

    public forgot(){
        this.passwordForm.markAllAsTouched();
        if (this.passwordForm.invalid)
            return;
        
        this.submitting = true;
        this.passwordSvc.forgotPassword(this.passwordForm.controls.email.value!)
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.messages = isSuccess(`Password reset email sent to ${this.passwordForm.controls.email.value}`);

                    this.passwordForm.reset();
                    this.submitting = false;
                },
                error: () => {
                    this.messages = isError('Unable to send reset email. Please try again later');
                    this.submitting = false;
                }
            });
    }
}