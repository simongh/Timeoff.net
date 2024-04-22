import { Component, DestroyRef, OnInit, numberAttribute, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { injectParams } from 'ngxtension/inject-params';

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

    protected get schedule() {
        return this.form.controls.schedule;
    }

    protected readonly submitting = signal(false);

    constructor(
        private destroyed: DestroyRef,
        private readonly usersSvc: UsersService,
        private readonly msgsSvc: MessagesService
    ) {}

    public ngOnInit(): void {
        this.usersSvc
            .getUser(this.id())
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe((user) => {
                this.usersSvc.fillForm(user);
            });
    }

    public update() {
        this.submitting.set(true);

        this.usersSvc
            .updateSchedule(this.id())
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.form.controls.scheduleOverride.setValue(true);
                    this.msgsSvc.isSuccess('Schedule Updated');
                    this.submitting.set(false);
                },
                error: (e: HttpErrorResponse) => {
                    this.msgsSvc.isError('Unabled to update schedule');
                    this.submitting.set(false);
                },
            });
    }

    public reset() {
        this.submitting.set(true);

        this.usersSvc
            .resetSchedule(this.id())
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: (schedule) => {
                    this.form.controls.scheduleOverride.setValue(false);
                    this.form.controls.schedule.setValue(schedule);

                    this.msgsSvc.isSuccess('Schedule updated');
                    this.submitting.set(false);
                },
                error: (e: HttpErrorResponse) => {
                    this.msgsSvc.isError('Unabled to update schedule');
                    this.submitting.set(false);
                },
            });
    }
}
