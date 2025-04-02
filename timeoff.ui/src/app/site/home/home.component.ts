import {
    Component,
    DestroyRef,
    OnInit,
    booleanAttribute,
    computed,
    inject,
    numberAttribute,
    signal,
} from '@angular/core';
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

import { LeaveRequestModel } from '@models/leave-request.model';
import { CalendarDayModel } from '@models/calendar-day.model';
import { NavigationComponent } from './navigation.component';
import { StatisticsComponent } from "./statistics.component";
import { AbsencesComponent } from "./absences.component";

@Component({
    templateUrl: 'home.component.html',
    imports: [
    FlashComponent,
    CommonModule,
    CalendarComponent,
    NavigationComponent,
    StatisticsComponent,
    AbsencesComponent
],
    providers: [CalendarService],
})
export class HomeComponent implements OnInit {
    readonly #destroyed = inject(DestroyRef);

    readonly #calendarSvc = inject(CalendarService);

    protected readonly name = signal('');

    protected readonly year = injectQueryParams((p) => numberAttribute(p['year'] ?? new Date().getFullYear()));

    protected readonly showFullYear = injectQueryParams((p) => booleanAttribute(p['showFullYear'] ?? false));

    protected readonly allowanceSummary = signal({} as AllowanceSummaryModel);

    protected readonly absences = signal<LeaveRequestModel[]>([]);

    protected readonly days = signal<CalendarDayModel[]>([]);

    protected readonly start = computed(() => {
        if (this.showFullYear()) {
            return new Date(this.year(), 0, 1);
        } else {
            return startOfMonth(new Date());
        }
    });

    public ngOnInit(): void {
        this.#calendarSvc
            .get(this.year())
            .pipe(takeUntilDestroyed(this.#destroyed))
            .subscribe((calendar) => {
                this.allowanceSummary.set(calendar.summary);
                this.days.set(calendar.days);
                this.absences.set(calendar.leaveRequested);
                this.name.set(`${calendar.firstName} ${calendar.lastName}`);
            });
    }
}
