import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

import { createScheduleForm } from '@components/schedule/schedule-form';

import { TeamModel } from './team.model';
import { UserModel } from './user.model';
import { SettingsModel } from './settings.model';
import { LeaveTypeModel } from './leave-type.model';
import { Country } from './country.model';
import { TimeZoneModel } from './time-zone.model';

type SettingsFormGroup = ReturnType<CompanyService['createForm']>;
export type LeaveTypeFormGroup = ReturnType<CompanyService['createLeaveTypeForm']>;

@Injectable()
export class CompanyService {
    readonly #fb = inject(FormBuilder);

    readonly #client = inject(HttpClient);

    public settingsForm: SettingsFormGroup = this.createForm();

    public leaveTypeForm: LeaveTypeFormGroup = this.createLeaveTypeForm();

    public leaveTypes = this.#fb.array<LeaveTypeFormGroup>([]);

    public first = this.#fb.control<number>(0);

    public getTeams() {
        return this.#client.get<TeamModel[]>('/api/company/teams');
    }

    public getUsers() {
        return this.#client.get<UserModel[]>('/api/company/users');
    }

    public getLeaveTypes() {
        return this.#client.get<LeaveTypeModel[]>('/api/company/leave-types');
    }

    public getSettings() {
        return this.#client.get<SettingsModel>('/api/company');
    }

    public saveSettings() {
        return this.#client.put<void>('/api/customer', this.settingsForm.value);
    }

    public saveSchedule() {
        const schedule = this.settingsForm.value.schedule!;

        return this.#client.put<void>('/api/company/schedule', schedule);
    }

    public updateLeaveTypes() {
        return this.#client.put<void>('/api/company/leave-types',{
            first: this.first.value,
            types: this.leaveTypes.value
        });
    }

    public fillLeaveTypes(types: LeaveTypeModel[]) {
        types.map((lt) => {
            this.leaveTypes.push(this.createLeaveTypeForm(lt));

            if (lt.first) {
                this.first.setValue(lt.id);
            }
        });
    }

    public resetLeaveTypeForm() {
        this.leaveTypeForm = this.createLeaveTypeForm();
    }

    public removeLeaveType(id: number){
        return this.#client.delete<void>(`/api/company/leave-types/${id}`);
    }

    public countries() {
        return this.#client.get<Country[]>('/api/company/countries');
    }

    public timeZones() {
        return this.#client.get<TimeZoneModel[]>('/api/company/time-zones');
    }

    public deleteCompany(companyName: string) {
        return this.#client.delete<void>('/api/company', {
            body: {
                name: companyName,
            },
        });
    }

    private createForm() {
        const form = this.#fb.group({
            name: ['', [Validators.required]],
            dateFormat: [''],
            country: ['GB', [Validators.required]],
            timeZone: ['', [Validators.required]],
            carryOver: [5, [Validators.min(0), Validators.max(1000)]],
            showHoliday: [false],
            hideTeamView: [false],
            schedule: createScheduleForm(this.#fb),
        });

        return form;
    }

    private createLeaveTypeForm(model?: LeaveTypeModel) {
        const form = this.#fb.group({
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
