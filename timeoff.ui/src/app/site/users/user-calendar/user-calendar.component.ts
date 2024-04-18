import { Component, DestroyRef, OnInit, computed, effect, numberAttribute, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { UserBreadcrumbComponent } from '../user-breadcrumb/user-breadcrumb.component';
import { injectParams } from 'ngxtension/inject-params';
import { injectQueryParams } from 'ngxtension/inject-query-params';

import { CalendarComponent } from '@components/calendar/calendar.component';
import { AllowanceBreakdownComponent } from '@components/allowance-breakdown/allowance-breakdown.component';
import { LeaveSummaryComponent } from '@components/leave-summary/leave-summary.component';

import { CalendarService } from '@services/calendar/calendar.service';
import { CalendarModel } from '@services/calendar/calendar.model';

import { UsersService } from '../users.service';
import { UserDetailsComponent } from '../user-details/user-details.component';

@Component({
    selector: 'user-calendar',
    standalone: true,
    templateUrl: './user-calendar.component.html',
    styleUrl: './user-calendar.component.scss',
    providers: [UsersService, CalendarService],
    imports: [
        RouterLink,
        UserBreadcrumbComponent,
        UserDetailsComponent,
        CalendarComponent,
        AllowanceBreakdownComponent,
        LeaveSummaryComponent,
    ],
})
export class UserCalendarComponent {
    public id = injectParams((p) => numberAttribute(p['id']));

    public currentYear = injectQueryParams((p) => numberAttribute(p['year'] ?? new Date().getFullYear()));

    public nextYear = computed(() => this.currentYear() + 1);

    public lastYear = computed(() => this.currentYear() - 1);

    public start = computed(() => new Date(this.currentYear(), 0, 1));

    public calendar: CalendarModel;

    constructor(
        private destroyed: DestroyRef,
        private readonly calendarSvc: CalendarService
    ) {
        this.calendar = {
            holidays: [],
            firstName: '',
            lastName: '',
            isActive: true,
            summary: {
                available: 0,
                carryOver: 0,
                used: 0,
                previousYear: 0,
                allowance: 0,
                isAccrued: false,
                adjustment: 0,
                employmentRangeAdjustment: 0,
                accruedAdjustment: 0,
                total: 0,
                remaining: 0,
                leaveSummary: [],
            },
        } as CalendarModel;

        effect(()=> {
            this.calendarSvc.get(this.currentYear(), this.id())
            .pipe(
                takeUntilDestroyed(this.destroyed),
            )
            .subscribe((calendar) => {
                this.calendar = calendar;
            });
        });
    }
}
