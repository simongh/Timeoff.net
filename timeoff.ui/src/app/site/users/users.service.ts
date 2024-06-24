import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { endOfToday, formatDate, isAfter, parseISO } from 'date-fns';

import { createScheduleForm } from '@components/schedule/schedule-form';

import { AllowanceSummaryModel } from '@services/calendar/allowance-summary.model';

import { dateString } from '@models/types';
import { ScheduleModel } from '@models/schedule.model';

import { UserModel } from './user.model';
import { UserAbsencesModel } from './user-absences.model';
import { UserListModel } from './user-list.model';

type UserFromGroup = ReturnType<UsersService['createUserForm']>;
type AdjustmentFormGroup = ReturnType<UsersService['createAdjustmentForm']>;

@Injectable()
export class UsersService {
    public readonly form: UserFromGroup;

    public adjustmentForm: AdjustmentFormGroup;

    public get fullName() {
        return `${this.form.controls.firstName.value} ${this.form.controls.lastName.value}`;
    }

    public get userEnabled() {
        const ending = this.form.controls.endDate.value;
        const isActive = this.form.controls.isActive.value ?? true;

        if (ending) {
            return isAfter(parseISO(ending), endOfToday()) && isActive;
        } else {
            return isActive;
        }
    }

    public id: number = 0;

    constructor(private readonly client: HttpClient, private readonly fb: FormBuilder) {
        this.form = this.createUserForm();
        this.adjustmentForm = this.createAdjustmentForm();
    }

    public getUsers(team: number | null) {
        const options = team
            ? {
                  params: new HttpParams().set('team', team),
              }
            : {};

        return this.client.get<UserListModel[]>('/api/users', options);
    }

    public addUser() {
        return this.client.post<void>('/api/users', this.form.value);
    }

    public getUser(id: number) {
        this.id = id;
        return this.client.get<UserModel>(`/api/users/${id}`);
    }

    public updateUser(id: number) {
        return this.client.put<void>(`/api/users/${id}`, {
            ...this.form.value,
            endDate: this.form.value.endDate == '' ? null : this.form.value.endDate,
        });
    }

    public deleteUser(id: number) {
        return this.client.delete<void>(`/api/users/${id}`);
    }

    public resetPassword(id: number) {
        return this.client.post<void>(`/api/users/${id}/reset-password`, {});
    }

    public updateSchedule(id: number) {
        return this.updateUserSchedule(id, this.form.value.schedule as ScheduleModel);
    }

    public resetSchedule(id: number) {
        return this.updateUserSchedule(id, null);
    }

    public getAbsences(id: number) {
        return this.client.get<UserAbsencesModel>(`/api/users/${id}/absences`);
    }

    public updateAdjustments(id: number) {
        return this.client.put<void>(`/api/users/${id}/adjustments`, this.adjustmentForm.value);
    }

    public fillForm(model: UserModel) {
        this.form.setValue({
            firstName: model.firstName,
            lastName: model.lastName,
            email: model.email,
            isAdmin: model.isAdmin,
            autoApprove: model.autoApprove,
            team: model.team,
            startDate: formatDate(model.startDate, 'yyyy-MM-dd'),
            endDate: model.endDate ? formatDate(model.endDate, 'yyyy-MM-dd') : null,
            isActive: model.isActive,
            schedule: model.schedule,
            scheduleOverride: model.scheduleOverride,
        });
    }

    public fillAdjustments(model: AllowanceSummaryModel) {
        this.adjustmentForm.setValue({
            carryOver: model.carryOver,
            adjustment: model.adjustment,
        });

        this.adjustmentForm.markAsUntouched();
    }

    private createUserForm() {
        const form = this.fb.group(
            {
                firstName: ['', Validators.required],
                lastName: ['', Validators.required],
                email: ['', [Validators.required, Validators.email]],
                team: [null as number | null],
                isAdmin: [false],
                autoApprove: [false],
                startDate: [null as dateString | null, Validators.required],
                endDate: [null as dateString | null],
                isActive: [true],
                schedule: createScheduleForm(this.fb),
                scheduleOverride: [false],
            },
            {
                validators: [],
            }
        );

        return form;
    }

    private updateUserSchedule(id: number, model: ScheduleModel | null) {
        return this.client.put<ScheduleModel>(`/api/users/${id}/schedule`, model);
    }

    private createAdjustmentForm() {
        const form = this.fb.group({
            carryOver: [0],
            adjustment: [0],
        });

        return form;
    }
}
