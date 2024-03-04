import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { TeamModel } from "./team.model";
import { FormBuilder, Validators } from "@angular/forms";

@Injectable()
export class TeamsService {
    public form = this.fb.group({
        name: ['', [Validators.required]],
        manager: [0, [Validators.required]],
        allowance: [0, [Validators.required, Validators.min(0), Validators.max(50)]],
        includePublicHolidays: false,
        accruedAllowance: false,
      });

    constructor(
        private readonly client: HttpClient,
        private readonly fb: FormBuilder) {}

    public getTeams() {
        return this.client.get<TeamModel[]>('/api/teams');
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
}