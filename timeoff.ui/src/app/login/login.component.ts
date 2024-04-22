import { Component, DestroyRef, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { injectQueryParams } from 'ngxtension/inject-query-params';

import { FlashComponent } from '@components/flash/flash.component';
import { ValidatorMessageComponent } from '@components/validator-message/validator-message.component';

import { AuthService } from '@services/auth/auth.service';
import { MessagesService } from '@services/messages/messages.service';
import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';

@Component({
    selector: 'login-apge',
    standalone: true,
    templateUrl: 'login.component.html',
    imports: [RouterLink, CommonModule, ReactiveFormsModule, FlashComponent, ValidatorMessageComponent],
    providers: [AuthService],
})
export class LoginComponent {
    protected readonly allowRegistrations = signal(true);

    protected readonly submitting = signal(false);

    protected get loginForm() {
        return this.authService.loginForm;
    }

    private readonly returnUrl = injectQueryParams((p) => p['returnUrl'] ?? '/');

    constructor(
        private readonly authService: AuthService,
        private readonly msgsSvc: MessagesService,
        private readonly currentUser: LoggedInUserService,
        private readonly router: Router,
        private destroyed: DestroyRef
    ) {}

    public login() {
        this.loginForm.markAllAsTouched();

        if (!this.loginForm.valid) {
            return;
        }

        this.submitting.set(true);
        this.authService
            .login()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: (r) => {
                    this.currentUser.clear();
                    if (r.success) {
                        this.currentUser.load(r);

                        this.router.navigateByUrl(this.returnUrl());
                    } else {
                        this.loginForm.controls.password.setValue('');
                        this.loginForm.markAsUntouched();

                        this.msgsSvc.isError('Unable to login');
                    }

                    this.submitting.set(false);
                },
                error: () => {
                    this.submitting.set(false);

                    this.msgsSvc.isError('Login failed');
                },
            });
    }
}
