import { isSameDay, isWeekend } from 'date-fns';
import { PublicHolidayModel } from '@models/public-holiday.model';

export class DayModel {
    public date: Date;

    public classes: string = '';

    public label: string | null = null;

    constructor(date: Date, holidays: PublicHolidayModel[]) {
        this.date = date;

        if (isWeekend(date)) {
            this.classes += ' weekend_cell';
        }
        if (isSameDay(date, new Date())) {
            this.classes += ' current_day_cell';
            this.label = 'Today';
        }

        const found = holidays.find((h) => isSameDay(date, h.date!));
        if (!!found) {
            this.classes += ' public_holiday_cell';
            this.label = `Public Holiday: ${found.name}`;
        }
    }
}
