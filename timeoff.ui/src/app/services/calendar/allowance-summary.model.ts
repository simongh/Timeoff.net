import { LeaveSummary } from "./leave-summary.model";

export interface AllowanceSummaryModel{
    available: number;
    allowance: number;
    carryOver: number;
    used: number;
    previousYear: number;
    adjustment: number;
    employmentRangeAdjustment: number;
    isAccrued: boolean;
    accruedAdjustment: number;
    remaining: number;
    total: number;
    leaveSummary: LeaveSummary[];
}