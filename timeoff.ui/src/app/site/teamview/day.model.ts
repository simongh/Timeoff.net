import { PublicHolidayModel } from "@models/public-holiday.model";
import { UserSummaryModel } from "./user-summary.model";
import { getDay, isSameDay, isWeekend } from "date-fns";

export class DayModel {
    public date: Date;

    public classes = '';

    public label : string | null = null;

    constructor(date: Date, user: UserSummaryModel, holidays: PublicHolidayModel[]) {
        this.date = date;    

        const weekday = (getDay(date) + 6) % 7
        if (!Object.values(user.schedule)[weekday]){
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