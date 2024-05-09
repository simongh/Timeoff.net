import { Component, computed, inject, numberAttribute } from '@angular/core';
import { RouterLink } from '@angular/router';
import { UserBreadcrumbComponent } from '../user-breadcrumb/user-breadcrumb.component';
import { injectParams } from 'ngxtension/inject-params';
import { injectQueryParams } from 'ngxtension/inject-query-params';
import { computedAsync } from 'ngxtension/computed-async';

import { CalendarComponent } from '@components/calendar/calendar.component';
import { AllowanceBreakdownComponent } from '@components/allowance-breakdown/allowance-breakdown.component';
import { LeaveSummaryComponent } from '@components/leave-summary/leave-summary.component';

import { CalendarService } from '@services/calendar/calendar.service';
import { CalendarModel } from '@services/calendar/calendar.model';
import { UsersService } from '../users.service';

@Component({
    selector: 'user-calendar',
    standalone: true,
    templateUrl: './user-calendar.component.html',
    styleUrl: './user-calendar.component.scss',
    providers: [CalendarService],
    imports: [
        RouterLink,
        UserBreadcrumbComponent,
        CalendarComponent,
        AllowanceBreakdownComponent,
        LeaveSummaryComponent,
    ],
})
export class UserCalendarComponent {
    private readonly calendarSvc = inject(CalendarService);

    private readonly userSvc = inject(UsersService);

    protected readonly currentYear = injectQueryParams((p) => numberAttribute(p['year'] ?? new Date().getFullYear()));

    protected readonly nextYear = computed(() => this.currentYear() + 1);

    protected readonly lastYear = computed(() => this.currentYear() - 1);

    protected readonly start = computed(() => {
        return new Date(this.currentYear(), 0, 1);
    });

    protected readonly calendar = computedAsync(() => this.calendarSvc.get(this.currentYear(), this.userSvc.id), {
        initialValue: {
            holidays: [],
            firstName: '',
            lastName: '',
            isActive: true,
            leaveRequested: [],
            days:[],
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
        } as CalendarModel,
    });
}
