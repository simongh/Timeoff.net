import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

import { TeamModel } from './team.model';

export type TeamFormGroup = TeamsService['form'];

@Injectable()
export class TeamsService {
    readonly #client = inject(HttpClient);

    public form = inject(FormBuilder).group({
        name: ['', [Validators.required]],
        manager: [null as number | null, [Validators.required]],
        allowance: [20, [Validators.required, Validators.min(0), Validators.max(50)]],
        includePublicHolidays: true,
        isAccruedAllowance: false,
    });

    public getTeams() {
        return this.#client.get<TeamModel[]>('/api/teams');
    }

    public get(id: number) {
        return this.#client.get<TeamModel>(`/api/teams/${id}`);
    }

    public delete(id: number) {
        return this.#client.delete<void>(`/api/teams/${id}`);
    }

    public update(id: number) {
        return this.#client.put<void>(`/api/teams/${id}`, this.form.value);
    }

    public create() {
        return this.#client.post<void>('/api/teams', this.form.value);
    }

    public reset() {
        this.form.reset({
            allowance: 20,
            includePublicHolidays: true,
            isAccruedAllowance: false,
        });
    }
}
