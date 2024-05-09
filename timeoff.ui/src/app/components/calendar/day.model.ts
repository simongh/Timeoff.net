import { isSameDay, isWeekend, isWithinInterval } from 'date-fns';
import { PublicHolidayModel } from '@models/public-holiday.model';
import { LeaveRequestModel } from '@models/leave-request.model';
import { datePart } from '@models/types';
import { CalendarDayModel } from '@models/calendar-day.model';

export class DayModel {
    public date: Date;

    public morningClasses: string[] = [];

    public afternoonClasses: string[] = [];

    public label: string | null = null;

    constructor(date: Date, days: CalendarDayModel[]) {
        this.date = date;

        if (isWeekend(date)) {
            this.morningClasses.push('weekend_cell');
            this.afternoonClasses.push('weekend_cell');
        }
        if (isSameDay(date, new Date())) {
            this.morningClasses.push('current_day_cell');
            this.afternoonClasses.push('current_day_cell');
            this.label = 'Today';
        }

        const found = days.filter((h) => isSameDay(date, h.date!));
        found.forEach((h) => {
            if (h.isHoliday) {
                this.morningClasses.push('public_holiday_cell');
                this.afternoonClasses.push('public_holiday_cell');
                this.label = `Public Holiday: ${h.name}`;
            } else {
                if (h.dayPart == datePart.morning || h.dayPart == datePart.wholeDay) {
                    this.morningClasses.push(h.colour!);
                }

                if (h.dayPart == datePart.afternoon || h.dayPart == datePart.wholeDay) {
                    this.afternoonClasses.push(h.colour!);
                }
                this.label = 'Approved leave';
            }
        });
    }
}
