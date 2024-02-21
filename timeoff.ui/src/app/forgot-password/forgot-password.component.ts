import { Component, DestroyRef, inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { FlashComponent } from "../components/flash/flash.component";
import { NgIf } from "@angular/common";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { AuthService } from "../services/auth/auth.service";

@Component({
    standalone: true,
    templateUrl: 'forgot-password.component.html',
    providers: [AuthService],
    imports: [FlashComponent, ReactiveFormsModule, NgIf,]
})
export class ForgotPasswordComponent {
    public passwordForm = this.fb.group({
        email: ['', [Validators.required, Validators.email]]
    })

    public messages: string[] = [];

    public submitting = false;

    private destroyed$ = inject(DestroyRef);

    constructor(
        private fb: FormBuilder,
        private passwordSvc: AuthService)
    {}

    public forgot(){
        this.passwordForm.markAllAsTouched();
        if (this.passwordForm.invalid)
            return;
        
        this.submitting = true;
        this.passwordSvc.forgotPassword(this.passwordForm.controls.email.value!)
            .pipe(takeUntilDestroyed(this.destroyed$))
            .subscribe(() => {
                this.messages = [`Password reset email sent to ${this.passwordForm.controls.email.value}`];
                this.submitting = false;
            });
    }
}