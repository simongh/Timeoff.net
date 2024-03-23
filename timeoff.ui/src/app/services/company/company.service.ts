import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TeamModel } from './team.model';
import { UserModel } from './user.model';
import { SettingsModel } from './settings.model';
import { ScheduleModel } from '../../models/schedule.model';
import { LeaveTypeModel } from './leave-type.model';
import { Country } from './country.model';
import { TimeZoneModel } from './time-zone.model';

type SettingsFormGroup = ReturnType<CompanyService['createForm']>;
export type LeaveTypeFormGroup = ReturnType<CompanyService['createLeaveTypeForm']>;

@Injectable()
export class CompanyService {
    public settingsForm: SettingsFormGroup;

    public leaveTypeForm: LeaveTypeFormGroup;

    public leaveTypes = this.fb.array<LeaveTypeFormGroup>([]);

    constructor(private readonly client: HttpClient, private readonly fb: FormBuilder) {
        this.settingsForm = this.createForm();
        this.leaveTypeForm = this.createLeaveTypeForm();
    }

    public getTeams() {
        return this.client.get<TeamModel[]>('/api/company/teams');
    }

    public getUsers() {
        return this.client.get<UserModel[]>('/api/company/users');
    }

    public getSettings() {
        return this.client.get<SettingsModel>('/api/company');
    }

    public saveSettings() {
        return this.client.put<void>('/api/customer', this.settingsForm.value);
    }

    public fillSchedule(schedule: ScheduleModel) {
        this.settingsForm.controls.schedule.clear();

        Object.values(schedule).map((s) =>
            this.settingsForm.controls.schedule.push(this.fb.control(s, { nonNullable: true }))
        );
    }

    public fillLeaveTypes(types: LeaveTypeModel[]) {
        types.map((lt) => {
            this.leaveTypes.push(this.createLeaveTypeForm(lt));
        });
    }

    public countries() {
        return this.client.get<Country[]>('/api/company/countries');
    }

    public timeZones() {
        return this.client.get<TimeZoneModel[]>('/api/company/time-zones');
    }

    private createForm() {
        const form = this.fb.group({
            name: ['', [Validators.required]],
            dateFormat: [''],
            country: ['GB', [Validators.required]],
            timeZone: ['', [Validators.required]],
            carryOver: [5, [Validators.min(0), Validators.max(1000)]],
            showHoliday: [false],
            hideTeamView: [false],
            schedule: this.fb.array<boolean>([]),
        });

        return form;
    }

    private createLeaveTypeForm(model?: LeaveTypeModel) {
        const form = this.fb.group({
            id: [model?.id],
            name: [model?.name, [Validators.required]],
            colour: [model?.colour ?? ''],
            useAllowance: [model?.useAllowance ?? true],
            autoApprove: [model?.autoApprove ?? false],
            limit: [model?.limit ?? 0],
            first: [model?.first ?? false],
        });

        return form;
    }
}
