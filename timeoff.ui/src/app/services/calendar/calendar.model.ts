import { PublicHolidayModel } from '../public-holidays/public-holiday.model';
import { AllowanceSummaryModel } from './allowance-summary.model';

export interface CalendarModel {
    holidays: PublicHolidayModel[];
    firstName: string;
    lastName: string;
    isActive: boolean;
    summary: AllowanceSummaryModel;
}
