import { Component, DestroyRef, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { switchMap } from 'rxjs';

import { DatePickerDirective } from '@components/date-picker.directive';
import { TeamSelectComponent } from '@components/team-select/team-select.component';
import { ValidatorMessageComponent } from '@components/validator-message/validator-message.component';

import { MessagesService } from '@services/messages/messages.service';

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
    public companyName = '';

    public dateFormat = 'yyyy-mm-dd';

    public id = 0;

    public get form() {
        return this.usersSvc.form;
    }

    public get fullName() {
        return this.usersSvc.fullName;
    }

    public get userEnabled() {
        return this.usersSvc.userEnabled;
    }

    public submitting = false;

    constructor(
        private readonly route: ActivatedRoute,
        private destroyed: DestroyRef,
        private readonly usersSvc: UsersService,
        private readonly msgsSvc: MessagesService
    ) {}

    public ngOnInit(): void {
        this.route.paramMap
            .pipe(
                takeUntilDestroyed(this.destroyed),
                switchMap((p) => {
                    this.id = Number.parseInt(p.get('id')!);

                    return this.usersSvc.getUser(this.id);
                })
            )
            .subscribe((user) => {
                this.usersSvc.fillForm(user);
            });
    }

    public save() {
        this.form.markAllAsTouched();

        if (this.form.invalid) {
            return;
        }

        this.submitting = true;

        this.usersSvc
            .updateUser(this.id)
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.submitting = false;
                    this.msgsSvc.isSuccess(`Details for ${this.fullName} were updated`);
                },
                error: (e: HttpErrorResponse) => {
                    if (e.status == 400) {
                        this.msgsSvc.hasErrors(e.error.errors);
                    } else {
                        this.msgsSvc.isError('Unable to update details');
                    }

                    this.submitting = false;
                },
            });
    }

    public resetPassword() {
        this.submitting = true;

        this.usersSvc
            .resetPassword(this.id)
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.submitting = false;
                    this.msgsSvc.isSuccess('Password reset');
                },
                error: () => {
                    this.msgsSvc.isError('Unable to reset password');
                    this.submitting = false;
                },
            });
    }
}
