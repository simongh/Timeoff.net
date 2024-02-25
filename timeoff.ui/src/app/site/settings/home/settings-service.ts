import { FormBuilder, Validators } from "@angular/forms";
import { LeaveTypeModel } from "./leave-type.model";
import { Injectable } from "@angular/core";

@Injectable()
export class SettingsService {
    public form = this.fb.group({
        name: ['', [Validators.required]],
        dateFormat: [''],
        country: ['GB', [Validators.required]],
        timezone: ['', [Validators.required]],
        carryOver: [5,[Validators.min(0), Validators.max(1000)]],
        showHoliday: [false],
        hideTeamView: [false]
    });

    public leaveTypeForm = this.fb.group({
        id:[0],
        name: ['', [Validators.required]],
        colour: [''],
        useAllowance: [true],
        autoApprove: [false],
        limit: [0],
    });

    public leaveTypes = this.fb.array([this.leaveTypeForm]);

    constructor(private readonly fb: FormBuilder)
    {}
}