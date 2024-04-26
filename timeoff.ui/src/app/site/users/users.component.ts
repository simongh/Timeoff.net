import { Component, DestroyRef, OnInit, numberAttribute, signal } from '@angular/core';
import { injectParams } from 'ngxtension/inject-params';
import { RouterOutlet, Router, RouterLink, RouterLinkActive } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';

import { MessagesService } from '@services/messages/messages.service';
import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';

import { FlashComponent } from '@components/flash/flash.component';

import { UsersService } from './users.service';
import { UserBreadcrumbComponent } from "./user-breadcrumb/user-breadcrumb.component";

@Component({
    standalone: true,
    templateUrl: 'users.component.html',
    providers: [UsersService],
    imports: [FlashComponent, RouterLink, RouterLinkActive, CommonModule, RouterOutlet, UserBreadcrumbComponent]
})
export class UsersComponent implements OnInit {
    protected readonly companyName = this.currentUser.companyName;

    protected readonly dateFormat = this.currentUser.dateFormat;

    protected get fullName() {
        return this.usersSvc.fullName;
    }

    protected get userEnabled() {
        return this.usersSvc.userEnabled;
    }

    protected readonly id = injectParams((p) => numberAttribute(p['id']));

    protected readonly submitting = signal(false);

    constructor(
        private destroyed: DestroyRef,
        private readonly usersSvc: UsersService,
        private readonly msgsSvc: MessagesService,
        private readonly currentUser: LoggedInUserService,
        private readonly router: Router
    ) {}

    public ngOnInit(): void {
        this.usersSvc
            .getUser(this.id())
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe((user) => {
                this.usersSvc.fillForm(user);
            });
    }

    public delete() {
        this.submitting.set(true);

        this.usersSvc
            .deleteUser(this.id())
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.msgsSvc.isSuccess('Employee data removed', true);
                    this.submitting.set(false);

                    this.router.navigateByUrl('/users');
                },
            });
    }
}
