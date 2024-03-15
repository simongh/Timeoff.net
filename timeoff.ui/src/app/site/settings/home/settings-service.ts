import {
    FormBuilder,
    FormControl,
    FormGroup,
    Validators,
} from '@angular/forms';
import { Injectable } from '@angular/core';

export interface LeaveTypeControls {
    id: FormControl<number | null>;
    name: FormControl<string | null>;
    colour: FormControl<string | null>;
    useAllowance: FormControl<boolean | null>;
    autoApprove: FormControl<boolean | null>;
    limit: FormControl<number | null>;
    first: FormControl<number | null>;
}

@Injectable()
export class SettingsService {
    public form = this.fb.group({
        name: ['', [Validators.required]],
        dateFormat: [''],
        country: ['GB', [Validators.required]],
        timezone: ['', [Validators.required]],
        carryOver: [5, [Validators.min(0), Validators.max(1000)]],
        showHoliday: [false],
        hideTeamView: [false],
    });

    public leaveTypeForm = this.fb.group({
        id: [0],
        name: ['', [Validators.required]],
        colour: [''],
        useAllowance: [true],
        autoApprove: [false],
        limit: [0],
        first: [null as number | null],
    });

    public leaveTypes = this.fb.array<FormGroup<LeaveTypeControls>>([]);

    constructor(private readonly fb: FormBuilder) {}
}
