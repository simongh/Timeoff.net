import { Component, DestroyRef, OnInit } from '@angular/core';
import { UserDetailsComponent } from '../user-details/user-details.component';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { UserBreadcrumbComponent } from '../user-breadcrumb/user-breadcrumb.component';
import { UsersService } from '../../../services/users/users.service';
import { combineLatest, switchMap } from 'rxjs';
import { endOfToday, formatDate, isAfter, parseISO } from 'date-fns';
import { TeamModel } from '../../../models/team.model';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { DatePickerDirective } from '../../../components/date-picker.directive';
import { MessagesService } from '../../../services/messages/messages.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
    selector: 'user-edit',
    standalone: true,
    templateUrl: './user-edit.component.html',
    styleUrl: './user-edit.component.sass',
    imports: [
        UserDetailsComponent,
        RouterLink,
        UserBreadcrumbComponent,
        ReactiveFormsModule,
        CommonModule,
        DatePickerDirective,
    ],
    providers: [UsersService],
})
export class UserEditComponent implements OnInit {
    public companyName = '';

    public dateFormat = 'yyyy-mm-dd';

    public id = 0;

    public get form() {
        return this.usersSvc.form;
    }

    public get fullName() {
        return `${this.form.controls.firstName.value} ${this.form.controls.lastName.value}`;
    }

    public get userEnabled() {
        const ending = this.form.controls.endDate.value;
        const isActive = this.form.controls.isActive.value;
        if (!!ending) {
            return isAfter(parseISO(ending), endOfToday()) && isActive;
        } else {
            return isActive;
        }
    }

    public teams: TeamModel[] = [];

    public submitting = false;

    constructor(
        private readonly route: ActivatedRoute,
        private destroyed: DestroyRef,
        private readonly usersSvc: UsersService,
        private readonly msgsSvc: MessagesService,
        private readonly router: Router,
    ) {}

    public ngOnInit(): void {
        combineLatest([
            this.route.paramMap.pipe(
                takeUntilDestroyed(this.destroyed),
                switchMap((p) => {
                    this.id = Number.parseInt(p.get('id')!);

                    return this.usersSvc.getUser(this.id);
                })
            ),
            this.usersSvc.getTeams(),
        ]).subscribe(([user, teams]) => {
            this.teams = teams;

            this.form.setValue({
                firstName: user.firstName,
                lastName: user.lastName,
                email: user.email,
                isAdmin: user.isAdmin,
                autoApprove: user.autoApprove,
                team: user.teamId,
                startDate: formatDate(user.startDate, 'yyyy-MM-dd'),
                endDate: user.endDate ? formatDate(user.endDate, 'yyyy-MM-dd') : null,
                isActive: user.isActive,
            });
        });
    }

    public save() {
        this.submitting = true;

        this.usersSvc
            .updateUser(this.id)
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.submitting = false;
                    this.msgsSvc.isSuccess(`Details for {request.Name} were updated`);
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

    public delete() {
        this.submitting = true;
        this.usersSvc.deleteUser(this.id)
        .pipe(takeUntilDestroyed(this.destroyed))
        .subscribe({
            next: () => {
                this.msgsSvc.isSuccess('Employee data removed');
                this.submitting = false;

                this.router.navigate(['users']);
            },
            error: (e: HttpErrorResponse) => {
                if (e.status == 400) {
                    this.msgsSvc.hasErrors(e.error.errors)
                }
                this.msgsSvc.isError('Unable to remove employee');
                this.submitting = false;
            }
        });
    }

    public resetPassword() {
        this.submitting = true;

        this.usersSvc.resetPassword(this.id)
        .pipe(takeUntilDestroyed(this.destroyed))
        .subscribe({
            next: () => {
                this.submitting = false;
                this.msgsSvc.isSuccess('Password reset');
            },
            error: () => {
                this.msgsSvc.isError('Unable to reset password');
                this.submitting = false;
            }
        })
    }
}
