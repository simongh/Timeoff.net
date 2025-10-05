import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

import { TeamModel } from '@app-types/team.model';

@Injectable({
  providedIn: 'root',
})
export class SiteApi {
  readonly #client = inject(HttpClient);

    public getTeams() {
        return this.#client.get<TeamModel[]>('/api/company/teams');
    }
}
