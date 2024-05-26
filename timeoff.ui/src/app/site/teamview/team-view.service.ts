import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { TeamViewModel } from './team-view.model';

@Injectable()
export class TeamViewService {
    constructor() {}

    public getSummaryForUsers(start: Date, team: number | null = null) {
        return of<TeamViewModel>({
            users: [{
                id: 0,
                name: 'test',
                user: {
                    id: 0,
                    name: 'test',
                },
                used: 0,
                days: [],
                schedule: {
                    monday: true,
                    tuesday: true,
                    wednesday: true,
                    thursday: true,
                    friday: true,
                    saturday:false,
                    sunday: false
                }
            }],
            schedule : {
                monday: true,
                tuesday: true,
                wednesday: true,
                thursday: true,
                friday: true,
                saturday:false,
                sunday: false
            },
        });
    }
}
