import { HttpClient, HttpParams } from '@angular/common/http';
import { TeamModel } from '../../models/team.model';
import { UserModel } from './user.model';
import { Injectable } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { dateString } from '../../components/types';

type UserFromGroup = ReturnType<UsersService['createUserForm']>;

@Injectable()
export class UsersService {
    public form: UserFromGroup;

    constructor(
        private readonly client: HttpClient,
        private readonly fb: FormBuilder
    ) {
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
            },
            {
                validators: [],
            }
        );

        return form;
    }
}
