import { PublicHolidayModel } from '@models/public-holiday.model';
import { LeaveRequestModel } from '@models/leave-request.model';
import { CalendarDayModel } from '@models/calendar-day.model';

import { AllowanceSummaryModel } from './allowance-summary.model';

export interface CalendarModel {
    firstName: string;
    lastName: string;
    isActive: boolean;
    summary: AllowanceSummaryModel;
    days: CalendarDayModel[];
    holidays: PublicHolidayModel[];
    leaveRequested: LeaveRequestModel[];
}
