import { PublicHolidayModel } from '@models/public-holiday.model';
import { LeaveRequestModel } from '@models/leave-request.model';

import { AllowanceSummaryModel } from './allowance-summary.model';

export interface CalendarModel {
    holidays: PublicHolidayModel[];
    firstName: string;
    lastName: string;
    isActive: boolean;
    summary: AllowanceSummaryModel;
    leaveRequested: LeaveRequestModel[];
}
