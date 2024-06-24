import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { UserSummaryModel } from './user-summary.model';

@Injectable()
export class TeamViewService {
    private readonly httpClient = inject(HttpClient);

    constructor() {}

    public getSummaryForUsers(start: Date, team: number | null = null) {
        const options = {
            params: new HttpParams()
                .set('year', start.getFullYear())
                .set('month', start.getMonth() + 1)
        };

        if (team){
            options.params = options.params.append('team',team);
        }

        return this.httpClient.get<UserSummaryModel[]>('/api/calendar/teams',options);
    }
}
