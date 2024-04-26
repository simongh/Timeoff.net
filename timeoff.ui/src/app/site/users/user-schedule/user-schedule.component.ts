import { Component, DestroyRef, numberAttribute, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { injectParams } from 'ngxtension/inject-params';

import { ScheduleComponent } from '@components/schedule/schedule.component';

import { MessagesService } from '@services/messages/messages.service';

import { UsersService } from '../users.service';
import { UserBreadcrumbComponent } from '../user-breadcrumb/user-breadcrumb.component';

@Component({
    selector: 'user-schedule',
    standalone: true,
    templateUrl: './user-schedule.component.html',
    styleUrl: './user-schedule.component.scss',
    imports: [
        UserBreadcrumbComponent,
        ReactiveFormsModule,
        CommonModule,
        RouterLink,
        ScheduleComponent,
    ],
})
export class UserScheduleComponent {
    protected get schedule() {
        return this.usersSvc.form.controls.schedule;
    }

    protected get override() {
        return this.usersSvc.form.value.scheduleOverride;
    }

    protected readonly submitting = signal(false);

    constructor(
        private destroyed: DestroyRef,
        private readonly usersSvc: UsersService,
        private readonly msgsSvc: MessagesService
    ) {}

    public update() {
        this.submitting.set(true);

        this.usersSvc
            .updateSchedule(this.usersSvc.id)
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.usersSvc.form.controls.scheduleOverride.setValue(true);
                    this.msgsSvc.isSuccess('Schedule Updated');
                    this.submitting.set(false);
                },
            });
    }

    public reset() {
        this.submitting.set(true);

        this.usersSvc
            .resetSchedule(this.usersSvc.id)
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: (schedule) => {
                    this.usersSvc.form.controls.scheduleOverride.setValue(false);
                    this.usersSvc.form.controls.schedule.setValue(schedule);

                    this.msgsSvc.isSuccess('Schedule updated');
                    this.submitting.set(false);
                },
            });
    }
}
