import { LeaveSummaryModel } from './leave-summary.model';

export interface AllowanceModel {
    id: number;
    firstName: string;
    lastName: string;
    allowanceUsed: number;
    leaveSummary: LeaveSummaryModel[];
    totals: number[];
}
