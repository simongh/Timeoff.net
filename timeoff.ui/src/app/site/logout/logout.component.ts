import { Component, DestroyRef, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Router } from '@angular/router';

import { AuthService } from '@services/auth/auth.service';
import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';

@Component({
    standalone: true,
    template: '',
})
export class LogoutComponent implements OnInit {
    constructor(
        private authService: AuthService,
        private currentUser: LoggedInUserService,
        private router: Router,
        private destroyed: DestroyRef
    ) {}

    public ngOnInit(): void {
        this.authService
            .logout()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe(() => {
                this.currentUser.clear();
                this.router.navigateByUrl('/');
            });
    }
}
