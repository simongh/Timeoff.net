import { Component, DestroyRef, OnInit, numberAttribute, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { injectParams } from 'ngxtension/inject-params';

import { DatePickerDirective } from '@components/date-picker.directive';
import { TeamSelectComponent } from '@components/team-select/team-select.component';
import { ValidatorMessageComponent } from '@components/validator-message/validator-message.component';

import { MessagesService } from '@services/messages/messages.service';
import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';

import { UsersService } from '../users.service';
import { UserBreadcrumbComponent } from '../user-breadcrumb/user-breadcrumb.component';
import { UserDetailsComponent } from '../user-details/user-details.component';

@Component({
    selector: 'user-edit',
    standalone: true,
    templateUrl: './user-edit.component.html',
    styleUrl: './user-edit.component.scss',
    providers: [UsersService],
    imports: [
        UserDetailsComponent,
        RouterLink,
        UserBreadcrumbComponent,
        ReactiveFormsModule,
        CommonModule,
        DatePickerDirective,
        TeamSelectComponent,
        ValidatorMessageComponent,
    ],
})
export class UserEditComponent implements OnInit {
    protected readonly companyName = this.currentUser.companyName;

    protected readonly dateFormat = this.currentUser.dateFormat;

    protected readonly id = injectParams((p) => numberAttribute(p['id']));

    protected get form() {
        return this.usersSvc.form;
    }

    protected get fullName() {
        return this.usersSvc.fullName;
    }

    protected get userEnabled() {
        return this.usersSvc.userEnabled;
    }

    protected readonly submitting = signal(false);

    constructor(
        private destroyed: DestroyRef,
        private readonly usersSvc: UsersService,
        private readonly msgsSvc: MessagesService,
        private readonly currentUser: LoggedInUserService
    ) {}

    public ngOnInit(): void {
        this.usersSvc
            .getUser(this.id())
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe((user) => {
                this.usersSvc.fillForm(user);
            });
    }

    public save() {
        this.form.markAllAsTouched();

        if (this.form.invalid) {
            return;
        }

        this.submitting.set(true);

        this.usersSvc
            .updateUser(this.id())
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.msgsSvc.isSuccess(`Details for ${this.fullName} were updated`);
                    this.currentUser.refresh();
                    this.submitting.set(false);
                },
            });
    }

    public resetPassword() {
        this.submitting.set(true);

        this.usersSvc
            .resetPassword(this.id())
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.submitting.set(false);
                    this.msgsSvc.isSuccess('Password reset');
                },
            });
    }
}
