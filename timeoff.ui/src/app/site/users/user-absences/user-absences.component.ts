import { Component, DestroyRef, OnInit, computed, numberAttribute, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { injectParams } from 'ngxtension/inject-params';

import { YesPipe } from '@components/yes.pipe';
import { RequestsListComponent } from '@components/requests-list/requests-list.component';
import { AllowanceBreakdownComponent } from "@components/allowance-breakdown/allowance-breakdown.component";
import { LeaveSummaryComponent } from "@components/leave-summary/leave-summary.component";

import { CalendarService } from '@services/calendar/calendar.service';
import { AllowanceSummaryModel } from '@services/calendar/allowance-summary.model';
import { MessagesService } from '@services/messages/messages.service';

import { LeaveRequestModel } from '@models/leave-request.model';

import { UsersService } from '../users.service';
import { UserBreadcrumbComponent } from '../user-breadcrumb/user-breadcrumb.component';
import { UserDetailsComponent } from '../user-details/user-details.component';

@Component({
    selector: 'user-absences',
    standalone: true,
    templateUrl: './user-absences.component.html',
    styleUrl: './user-absences.component.scss',
    providers: [UsersService, CalendarService],
    imports: [
        UserDetailsComponent,
        UserBreadcrumbComponent,
        CommonModule,
        YesPipe,
        RequestsListComponent,
        ReactiveFormsModule,
        RouterLink,
        AllowanceBreakdownComponent,
        LeaveSummaryComponent
    ]
})
export default class UserAbsencesComponent implements OnInit {
    protected readonly id = injectParams((p) => numberAttribute(p['id']));

    protected get form() {
        return this.usersSvc.adjustmentForm;
    }

    protected readonly name = signal('');

    protected readonly isActive = signal(false);

    protected readonly summary = signal({} as AllowanceSummaryModel);

    protected readonly usedPercent = computed(() => (this.summary().used / this.summary().total) * 100);

    protected readonly remainingPercent = computed(() => (this.summary().remaining / this.summary().total) * 100);

    protected groupedRequests = computed(() => {
        return Object.entries(
            this.leave().reduce((groups, item) => {
                const year = item.startDate.getFullYear();

                (groups[year] ||= []).push(item);

                return groups;
            }, {} as Record<number, LeaveRequestModel[]>)
        );
    });

    protected readonly submitting = signal(false);

    private readonly leave = signal<LeaveRequestModel[]>([]);

    constructor(
        private destroyed: DestroyRef,
        private readonly usersSvc: UsersService,
        private readonly calendarSvc: CalendarService,
        private readonly msgsSvc: MessagesService
    ) {}

    public ngOnInit(): void {
        this.calendarSvc
            .get(new Date().getFullYear(), this.id())
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe((calendar) => {
                this.summary.set(calendar.summary);
                this.name.set(`${calendar.firstName} ${calendar.lastName}`);
                this.isActive.set(calendar.isActive);

                this.usersSvc.fillAdjustments(calendar.summary);
            });
    }

    public save() {
        this.submitting.set(true);

        this.usersSvc
            .updateAdjustments(this.id())
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.msgsSvc.isSuccess('Adjustments save');

                    this.summary.update((s) => {
                        const diff = s.adjustment - this.form.value.adjustment!;
                        s.total = s.total - diff;
                        s.remaining = s.remaining - diff;

                        s.adjustment = this.form.value.adjustment!;
                        return { ...s };
                    });

                    this.submitting.set(false);
                },
                error: (e: HttpErrorResponse) => {
                    this.msgsSvc.isError('Unable to save adjustments');
                    this.submitting.set(false);
                },
            });
    }
}
