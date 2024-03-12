import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { TeamModel } from "./team.model";
import { FormBuilder, Validators } from "@angular/forms";
import { UserModel } from "../../models/user.model";

export type TeamFormGroup = ReturnType<TeamsService['createForm']>

@Injectable()
export class TeamsService {
    public form: TeamFormGroup;

    constructor(
        private readonly client: HttpClient,
        private readonly fb: FormBuilder) {
            this.form = this.createForm();
        }

    public getTeams() {
        return this.client.get<TeamModel[]>('/api/teams');
    }

    public getUsers() {
        return this.client.get<UserModel[]>('/api/teams/users');
    }

    public get(id: number) {
        return this.client.get<TeamModel>(`/api/teams/${id}`);
    }

    public delete(id: number) {
        return this.client.delete<void>(`/api/teams/${id}`);
    }

    public update(id: number) {
        return this.client.put<void>(`/api/teams/${id}`, this.form.value);
    }

    public create() {
        return this.client.post<void>('/api/teams', this.form.value);
    }

    public reset() {
        this.form.reset({
            allowance: 20,
            includePublicHolidays: true,
            isAccruedAllowance: false,
        });
    }

    private createForm() {
        return this.fb.group({
            name: ['', [Validators.required]],
            managerId: [0, [Validators.required, Validators.min(1)]],
            allowance: [20, [Validators.required, Validators.min(0), Validators.max(50)]],
            includePublicHolidays: true,
            isAccruedAllowance: false,
          });
    }
}