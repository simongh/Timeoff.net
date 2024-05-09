import { Component, DestroyRef, OnInit, booleanAttribute, computed, numberAttribute, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { startOfMonth } from 'date-fns';
import { injectQueryParams } from 'ngxtension/inject-query-params';

import { FlashComponent } from '@components/flash/flash.component';
import { CalendarComponent } from '@components/calendar/calendar.component';
import { AllowanceBreakdownComponent } from '@components/allowance-breakdown/allowance-breakdown.component';
import { LeaveSummaryComponent } from '@components/leave-summary/leave-summary.component';

import { AllowanceSummaryModel } from '@services/calendar/allowance-summary.model';
import { CalendarService } from '@services/calendar/calendar.service';

import { PublicHolidayModel } from '@models/public-holiday.model';
import { LeaveRequestModel } from '@models/leave-request.model';
import { CalendarDayModel } from '@models/calendar-day.model';

@Component({
    standalone: true,
    templateUrl: 'home.component.html',
    imports: [
        FlashComponent,
        CommonModule,
        RouterLink,
        CalendarComponent,
        AllowanceBreakdownComponent,
        LeaveSummaryComponent,
    ],
    providers: [CalendarService],
})
export class HomeComponent implements OnInit {
    protected readonly name = signal('');

    protected readonly year = injectQueryParams((p) => numberAttribute(p['year'] ?? new Date().getFullYear()));

    protected readonly nextYear = computed(() => this.year() + 1);

    protected readonly lastYear = computed(() => this.year() - 1);

    protected readonly showFullYear = injectQueryParams((p) => booleanAttribute(p['showFullYear'] ?? false));

    protected readonly allowanceSummary = signal({} as AllowanceSummaryModel);

    protected readonly absences = signal<LeaveRequestModel[]>([]);

    protected readonly days = signal<CalendarDayModel[]>([]);

    protected readonly managerName = signal('manager');

    protected readonly managerEmail = signal('manager@email');

    protected readonly teamName = signal('team');

    protected readonly teamId = signal(0);

    protected readonly start = computed(() => {
        if (this.showFullYear()) {
            return new Date(this.year(), 0, 1);
        } else {
            return startOfMonth(new Date());
        }
    });

    constructor(private readonly destroyed: DestroyRef, private readonly calendarSvc: CalendarService) {}

    public ngOnInit(): void {
        this.calendarSvc
            .get(this.year())
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe((calendar) => {
                this.allowanceSummary.set(calendar.summary);
                this.days.set(calendar.days);
                this.absences.set(calendar.leaveRequested);
                this.name.set(`${calendar.firstName} ${calendar.lastName}`);
            });
    }
}
