import { Component, DestroyRef, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { switchMap } from 'rxjs';

import { ScheduleComponent } from '@components/schedule/schedule.component';

import { MessagesService } from '@services/messages/messages.service';

import { UsersService } from '../users.service';
import { UserDetailsComponent } from '../user-details/user-details.component';
import { UserBreadcrumbComponent } from '../user-breadcrumb/user-breadcrumb.component';

@Component({
    selector: 'user-schedule',
    standalone: true,
    templateUrl: './user-schedule.component.html',
    styleUrl: './user-schedule.component.scss',
    providers: [UsersService],
    imports: [
        UserDetailsComponent,
        UserBreadcrumbComponent,
        ReactiveFormsModule,
        CommonModule,
        RouterLink,
        ScheduleComponent,
    ],
})
export class UserScheduleComponent implements OnInit {
    public id!: number;

    public get form() {
        return this.usersSvc.form;
    }

    public get fullName() {
        return this.usersSvc.fullName;
    }

    public get userEnabled() {
        return this.usersSvc.userEnabled;
    }

    public get schedule() {
        return this.form.controls.schedule;
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

    public update() {
        this.submitting = true;

        this.usersSvc
            .updateSchedule(this.id)
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.form.controls.scheduleOverride.setValue(true);
                    this.msgsSvc.isSuccess('Schedule Updated');
                    this.submitting = false;
                },
                error: (e: HttpErrorResponse) => {
                    this.msgsSvc.isError('Unabled to update schedule');
                    this.submitting = false;
                },
            });
    }

    public reset() {
        this.submitting = true;

        this.usersSvc
            .resetSchedule(this.id)
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: (schedule) => {
                    this.form.controls.scheduleOverride.setValue(false);
                    this.usersSvc.fillSchedule(schedule);

                    this.msgsSvc.isSuccess('Schedule updated');
                    this.submitting = false;
                },
                error: (e: HttpErrorResponse) => {
                    this.msgsSvc.isError('Unabled to update schedule');
                    this.submitting = false;
                },
            });
    }
}
