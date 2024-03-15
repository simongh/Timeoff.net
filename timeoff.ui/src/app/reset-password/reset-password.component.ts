import { Component, DestroyRef, OnInit } from '@angular/core';
import { FlashComponent } from '../components/flash/flash.component';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { compareValidator } from '../components/validators';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../services/auth/auth.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ValidatorMessageComponent } from '../components/validator-message/validator-message.component';
import { HttpErrorResponse } from '@angular/common/http';
import { MessagesService } from '../services/messages/messages.service';

@Component({
    standalone: true,
    templateUrl: './reset-password.component.html',
    styleUrl: './reset-password.component.sass',
    imports: [
        FlashComponent,
        CommonModule,
        ReactiveFormsModule,
        ValidatorMessageComponent,
    ],
})
export class ResetPasswordComponent implements OnInit {
    public passwordForm = this.fb.group(
        {
            current: [''],
            password: ['', [Validators.required, Validators.minLength(8)]],
            confirmPassword: ['', []],
        },
        {
            validators: [compareValidator('password', 'confirmPassword')],
        }
    );

    public showCurrent!: boolean;

    public submitting = false;

    private token: string | null = null;

    constructor(
        private readonly fb: FormBuilder,
        private route: ActivatedRoute,
        private readonly authSvc: AuthService,
        private readonly msgsSvc: MessagesService,
        private destroyed: DestroyRef
    ) {}

    public ngOnInit(): void {
        this.showCurrent = this.authSvc.isUserLoggedIn;

        this.route.queryParamMap
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe((p) => {
                if (p.has('t')) {
                    this.token = p.get('t');
                } else {
                    this.token = null;
                }

                if (this.showCurrent) {
                    this.passwordForm.controls.current.addValidators(
                        Validators.required
                    );
                } else {
                    if (!this.token) {
                        this.msgsSvc.isError('Invalid reset link');
                        this.submitting = true;
                    }
                }
            });
    }

    public save() {
        this.passwordForm.markAllAsTouched();

        if (this.passwordForm.invalid) {
            return;
        }

        this.submitting = true;

        const f = this.passwordForm.value;
        this.authSvc
            .resetPassword({
                password: f.current ? f.current : null,
                newPassword: f.password!,
                confirmPassword: f.confirmPassword!,
                token: this.token,
            })
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: (r) => {
                    this.msgsSvc.isSuccess('Password updated successfully');
                    this.submitting = false;
                    this.passwordForm.reset();
                },
                error: (e: HttpErrorResponse) => {
                    if (e.status === 400) {
                        this.msgsSvc.hasErrors(e.error.errors);
                    } else
                        this.msgsSvc.isError(
                            'Unable to reset password. Please try again later'
                        );
                    this.submitting = false;
                },
            });
    }
}
