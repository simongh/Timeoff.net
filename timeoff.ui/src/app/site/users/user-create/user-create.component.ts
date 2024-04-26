import { Component, DestroyRef, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';

import { FlashComponent } from '@components/flash/flash.component';
import { ValidatorMessageComponent } from '@components/validator-message/validator-message.component';
import { DatePickerDirective } from '@components/date-picker.directive';
import { TeamSelectComponent } from '@components/team-select/team-select.component';

import { MessagesService } from '@services/messages/messages.service';

import { UsersService } from '../users.service';
import { UserBreadcrumbComponent } from '../user-breadcrumb/user-breadcrumb.component';
import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';

@Component({
    selector: 'user-create',
    standalone: true,
    providers: [UsersService],
    templateUrl: './user-create.component.html',
    styleUrl: './user-create.component.scss',
    imports: [
        RouterLink,
        ReactiveFormsModule,
        FlashComponent,
        CommonModule,
        ValidatorMessageComponent,
        DatePickerDirective,
        UserBreadcrumbComponent,
        TeamSelectComponent,
    ],
})
export class UserCreateComponent {
    protected get form() {
        return this.usersSvc.form;
    }

    protected readonly dateFormat = this.currentUser.dateFormat;

    protected readonly companyName = this.currentUser.companyName;

    protected readonly submitting = signal(false);

    constructor(
        private readonly currentUser: LoggedInUserService,
        private readonly usersSvc: UsersService,
        private readonly messagesSvc: MessagesService,
        private readonly router: Router,
        private destroyed: DestroyRef
    ) {}

    public add() {
        this.form.markAllAsTouched();

        if (this.form.invalid) {
            return;
        }

        this.submitting.set(true);

        this.usersSvc
            .addUser()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.messagesSvc.isSuccess('New user account successfully added', true);
                    this.currentUser.refresh();
                    this.router.navigateByUrl('/users');
                },
            });
    }
}
