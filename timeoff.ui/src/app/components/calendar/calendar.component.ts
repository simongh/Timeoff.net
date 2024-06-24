import { CommonModule } from '@angular/common';
import { Component, computed, input } from '@angular/core';
import {
    addMonths,
    eachDayOfInterval,
    eachMonthOfInterval,
    eachWeekOfInterval,
    endOfMonth,
    endOfWeek,
    getDay,
} from 'date-fns';

import { PublicHolidayModel } from '@models/public-holiday.model';

import { DayModel } from './day.model';
import { LeaveRequestModel } from '@models/leave-request.model';
import { CalendarDayModel } from '@models/calendar-day.model';

@Component({
    selector: 'calendar',
    standalone: true,
    templateUrl: 'calendar.component.html',
    styleUrl: 'calendar.component.scss',
    imports: [CommonModule],
})
export class CalendarComponent {
    public readonly monthCount = input(0);

    public readonly monthFormat = input('');

    public readonly start = input(new Date());

    public readonly colStyle = input('col-md-3');

    public readonly holidays = input<PublicHolidayModel[]>([]);

    public readonly absences = input<LeaveRequestModel[]>([]);

    public readonly days = input<CalendarDayModel[]>([]);

    protected readonly weeks = computed(() => {
        return eachMonthOfInterval({
            start: this.start(),
            end: addMonths(this.start(), this.monthCount() - 1),
        }).map((m) => ({
            date: m,
            padding: (getDay(m) + 6) % 7,
            weeks: eachWeekOfInterval(
                {
                    start: m,
                    end: endOfMonth(m),
                },
                { weekStartsOn: 1 }
            ).map((w, i) => ({
                num: i,
                days: eachDayOfInterval({
                    start: w,
                    end: endOfWeek(w, { weekStartsOn: 1 }),
                })
                    .filter((day) => day.getMonth() == m.getMonth())
                    .map((day) => {
                        return new DayModel(day, this.days());
                    }),
            })),
        }));
    });
}
