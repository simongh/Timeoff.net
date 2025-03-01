import { getDay, isSameDay } from 'date-fns';

import { LeaveStatus, datePart } from '@models/types';

import { UserSummaryModel } from './user-summary.model';

export class DayModel {
    public date: Date;

    public morningClasses: string[] = [];

    public afternoonClasses: string[] = [];

    public label: string | null = null;

    constructor(date: Date, user: UserSummaryModel) {
        this.date = date;

        const weekday = (getDay(date) + 6) % 7;
        if (!Object.values(user.schedule)[weekday]) {
            this.morningClasses.push(' weekend_cell');
            this.afternoonClasses.push('weekend_cell');
        }

        if (isSameDay(date, new Date())) {
            this.morningClasses.push(' current_day_cell');
            this.afternoonClasses.push(' current_day_cell');
            this.label = 'Today';
        }

        const found = user.days.filter((h) => isSameDay(date, h.date!));
        found.forEach((h) => {
            if (h.isHoliday) {
                this.morningClasses.push('public_holiday_cell');
                this.afternoonClasses.push('public_holiday_cell');
                this.label = `Public Holiday: ${h.name}`;
            } else {
                const colour = h.status == LeaveStatus.New ? 'leave_cell_pending' : h.colour!;

                this.label = 'Holiday';
                if (h.dayPart == datePart.morning || h.dayPart == datePart.wholeDay) {
                    this.morningClasses.push(colour);
                }

                if (h.dayPart == datePart.afternoon || h.dayPart == datePart.wholeDay) {
                    this.afternoonClasses.push(colour);
                }

                if (h.status == LeaveStatus.New) {
                    this.label += ': New absence waiting approval';
                } else if (h.status == LeaveStatus.PendingRevoke) {
                    this.label += ': Revoked waiting for approval';
                } else if (h.status == LeaveStatus.Approved) {
                    this.label += ': Approved absence';
                }
            }
        });
    }
}
