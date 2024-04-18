import {
    Component,
    DestroyRef,
    OnInit,
    booleanAttribute,
    computed,
    effect,
    numberAttribute,
    signal,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { startOfMonth, startOfYear } from 'date-fns';
import { injectQueryParams } from 'ngxtension/inject-query-params';

import { FlashComponent } from '@components/flash/flash.component';
import { CalendarComponent } from '@components/calendar/calendar.component';
import { AllowanceBreakdownComponent } from '@components/allowance-breakdown/allowance-breakdown.component';
import { LeaveSummaryComponent } from '@components/leave-summary/leave-summary.component';

import { AllowanceSummaryModel } from '@services/calendar/allowance-summary.model';
import { CalendarService } from '@services/calendar/calendar.service';

import { PublicHolidayModel } from '@models/public-holiday.model';

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
export class HomeComponent {
    public name = signal('');

    protected year = injectQueryParams((p) => numberAttribute(p['year'] ?? new Date().getFullYear()));

    protected nextYear = computed(() => this.year() + 1);

    protected lastYear = computed(() => this.year() - 1);

    protected showFullYear = injectQueryParams((p) => booleanAttribute(p['showFullYear'] ?? false));

    public allowanceSummary = signal({} as AllowanceSummaryModel);

    public holidays = signal<PublicHolidayModel[]>([]);

    protected managerName = signal('manager');

    protected managerEmail = signal('manager@email');

    protected teamName = signal('team');

    protected teamId = signal(0);

    public start = computed(() => {
        if (this.showFullYear()) {
            return new Date(this.year(), 0, 1);
        } else {
            return startOfMonth(new Date());
        }
    });

    constructor(
        private readonly destroyed: DestroyRef,
        private readonly calendarSvc: CalendarService
    ) {
        effect(() => {
            this.calendarSvc
                .get(this.year())
                .pipe(takeUntilDestroyed(this.destroyed))
                .subscribe((calendar) => {
                    this.allowanceSummary.set(calendar.summary);
                    this.holidays.set(calendar.holidays);
                    this.name.set(`${calendar.firstName} ${calendar.lastName}`);
                });
        });
    }
}
