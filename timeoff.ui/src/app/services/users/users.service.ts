import { HttpClient, HttpParams } from '@angular/common/http';
import { TeamModel } from '../../models/team.model';
import { UserModel } from './user.model';
import { Injectable } from '@angular/core';
import { FormArray, FormBuilder, FormControl, Validators } from '@angular/forms';
import { dateString } from '../../components/types';
import { endOfToday, formatDate, isAfter, parseISO } from 'date-fns';
import { ScheduleModel } from '../../models/schedule.model';
import { WeekDay } from '@angular/common';

type UserFromGroup = ReturnType<UsersService['createUserForm']>;

@Injectable()
export class UsersService {
    public form: UserFromGroup;

    public get fullName() {
        return `${this.form.controls.firstName.value} ${this.form.controls.lastName.value}`;
    }

    public get userEnabled() {
        const ending = this.form.controls.endDate.value;
        const isActive = this.form.controls.isActive.value ?? true;

        if (!!ending) {
            return isAfter(parseISO(ending), endOfToday()) && isActive;
        } else {
            return isActive;
        }
    }

    constructor(private readonly client: HttpClient, private readonly fb: FormBuilder) {
        this.form = this.createUserForm();
    }

    public getTeams() {
        return this.client.get<TeamModel[]>('/api/users/teams');
    }

    public getUsers(team: number | null) {
        const options = team
            ? {
                  params: new HttpParams().set('team', team),
              }
            : {};

        return this.client.get<UserModel[]>('/api/users', options);
    }

    public addUser() {
        return this.client.post<void>('/api/users', this.form.value);
    }

    public getUser(id: number) {
        return this.client.get<UserModel>(`/api/users/${id}`);
    }

    public updateUser(id: number) {
        return this.client.put<void>(`/api/users/${id}`, this.form.value);
    }

    public deleteUser(id: number) {
        return this.client.delete<void>(`/api/users/${id}`);
    }

    public resetPassword(id: number) {
        return this.client.post<void>(`/api/users/${id}/reset-password`, {});
    }

    public updateSchedule(id: number) {
        return this.updateUserSchedule(id, {
            monday: this.form.value.schedule![0]!,
            tuesday: this.form.value.schedule![1]!,
            wednesday: this.form.value.schedule![2]!,
            thursday: this.form.value.schedule![3]!,
            friday: this.form.value.schedule![4]!,
            saturday: this.form.value.schedule![5]!,
            sunday: this.form.value.schedule![6]!,
        });
    }

    public resetSchedule(id: number) {
        return this.updateUserSchedule(id, null);
    }

    public fillForm(model: UserModel) {
        this.form.setValue({
            firstName: model.firstName,
            lastName: model.lastName,
            email: model.email,
            isAdmin: model.isAdmin,
            autoApprove: model.autoApprove,
            team: model.teamId,
            startDate: formatDate(model.startDate, 'yyyy-MM-dd'),
            endDate: model.endDate ? formatDate(model.endDate, 'yyyy-MM-dd') : null,
            isActive: model.isActive,
            schedule: [],
            scheduleOverride: model.scheduleOverride,
        });

        this.fillSchedule(model.schedule);
    }

    public fillSchedule(schedule: ScheduleModel) {
        this.form.controls.schedule.clear();

        Object.values(schedule).map((s) => this.form.controls.schedule.push(this.fb.control(s, { nonNullable: true })));
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
                schedule: this.fb.array<boolean>([]),
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
}
