import { AllowanceSummaryModel } from '@services/calendar/allowance-summary.model';

import { LeaveRequestModel } from '@models/leave-request.model';

export interface UserAbsencesModel {
    id: number;
    firstName: string;
    lastName: string;
    isActive: boolean;
    summary: AllowanceSummaryModel;
    absences: LeaveRequestModel[];
}
