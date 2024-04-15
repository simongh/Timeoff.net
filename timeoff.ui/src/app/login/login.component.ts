import { Component, DestroyRef } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { combineLatest } from 'rxjs';

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
    public allowRegistrations: boolean = true;

    public submitting = false;

    public get loginForm() {
        return this.authService.loginForm;
    }

    constructor(
        private readonly authService: AuthService,
        private readonly msgsSvc: MessagesService,
        private readonly currentUser: LoggedInUserService,
        private readonly route: ActivatedRoute,
        private readonly router: Router,
        private destroyed: DestroyRef
    ) {}

    public login() {
        this.loginForm.markAllAsTouched();

        if (!this.loginForm.valid) return;

        this.submitting = true;
        combineLatest([this.authService.login(), this.route.queryParamMap])
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: ([r, q]) => {
                    this.currentUser.clear();
                    if (r.success) {
                        this.currentUser.load(r);

                        const url = q.get('returnUrl') || '/';
                        this.router.navigateByUrl(url);
                    } else {
                        this.loginForm.controls.password.setValue('');
                        this.loginForm.markAsUntouched();

                        this.msgsSvc.isError('Unable to login');
                    }

                    this.submitting = false;
                },
                error: () => {
                    this.submitting = false;

                    this.msgsSvc.isError('Login failed');
                },
            });
    }
}
