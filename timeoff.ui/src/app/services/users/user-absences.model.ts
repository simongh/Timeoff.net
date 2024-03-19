import { LeaveRequestModel } from "../../models/leave-request.model";
import { AllowanceSummaryModel } from "../calendar/allowance-summary.model";

export interface UserAbsencesModel{
    id: number;
    firstName: string;
    lastName: string;
    isActive: boolean;
    summary: AllowanceSummaryModel
    absences: LeaveRequestModel[];
}