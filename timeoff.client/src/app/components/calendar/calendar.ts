import { Component, computed, input } from '@angular/core';
import { NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import {
  addMonths,
  eachDayOfInterval,
  eachMonthOfInterval,
  eachWeekOfInterval,
  endOfMonth,
  endOfWeek,
  formatDate,
  getDay,
  isSameDay,
  isWeekend,
} from 'date-fns';

import { CalendarDayModel } from '@app-types/calendar-day.model';
import { dateString } from '@app-types/dateString';

@Component({
  selector: 'ton-calendar',
  imports: [NgbTooltip],
  templateUrl: './calendar.html',
  styleUrl: './calendar.scss',
})
export class Calendar {
  public readonly monthCount = input(3);

  public readonly monthFormat = input('MMM');

  public readonly start = input<dateString>('');

  public readonly colStyle = input('col-md-3');

  public readonly events = input<CalendarDayModel[]>([]);

  private readonly today = new Date();

  protected readonly weeks = computed(() => {
    return eachMonthOfInterval({
      start: this.start(),
      end: addMonths(this.start(), this.monthCount() - 1),
    }).map((m) => this.formatMonth(m));
  });

  private formatMonth(date: Date) {
    return {
      start: date,
      month: formatDate(date, this.monthFormat()),
      padding: (getDay(date) + 6) % 7,
      weeks: eachWeekOfInterval(
        {
          start: date,
          end: endOfMonth(date),
        },
        { weekStartsOn: 1 },
      ).map((w, i) => this.formatWeek(w, date.getMonth(), i)),
    };
  }

  private formatWeek(date: Date, month: number, index: number) {
    return {
      num: index,
      start: date,
      days: eachDayOfInterval({
        start: date,
        end: endOfWeek(date, { weekStartsOn: 1 }),
      })
        .filter((day) => day.getMonth() == month)
        .map((day) => this.formatDay(day)),
    };
  }

  private formatDay(day: Date) {
    const morning: string[] = [];
    const afternoon: string[] = [];

    let label = '';

    if (isWeekend(day)) {
      morning.push('weekend_cell');
      afternoon.push('weekend_cell');
    }

    if (isSameDay(day, this.today)) {
      morning.push('current_day_cell');
      afternoon.push('current_day_cell');
      label = 'Today';
    }

    const found = this.events().filter((h) => isSameDay(day, h.date));
    found.forEach((h) => {
      if (h.isHoliday) {
        morning.push('public_holiday_cell');
        afternoon.push('public_holiday_cell');
        label = h.name
      }
    })

    return {
      start: day,
      day: formatDate(day, 'dd'),
      label: label,
      morning: morning,
      afternoon: afternoon,
    };
  }
}
