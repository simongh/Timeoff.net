import { isSameDay, isWeekend, isWithinInterval } from 'date-fns';
import { PublicHolidayModel } from '@models/public-holiday.model';
import { LeaveRequestModel } from '@models/leave-request.model';
import { datePart } from '@models/types';

export class DayModel {
    public date: Date;

    public morningClasses: string[] = [];
    
    public afternoonClasses: string[] = [];

    public label: string | null = null;

    constructor(date: Date, holidays: PublicHolidayModel[], absences: LeaveRequestModel[]) {
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

        const found = holidays.find((h) => isSameDay(date, h.date!));
        if (!!found) {
            this.morningClasses.push('public_holiday_cell');
            this.afternoonClasses.push('public_holiday_cell');
            this.label = `Public Holiday: ${found.name}`;
        }

        const absence = absences.find((a) => isWithinInterval(date,{
            start: a.startDate,
            end: a.endDate
        }));
        if (!!absence) {
            if (absence.startPart == datePart.morning || absence.startPart == datePart.wholeDay) {
            this.morningClasses.push(absence.type.colour);
            }

            if (absence.endPart == datePart.afternoon || absence.endPart == datePart.wholeDay) {
                this.afternoonClasses.push(absence.type.colour);
            }

            this.label = 'Approved leave';
        }
    }
}
