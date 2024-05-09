import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { TeamViewModel } from './team-view.model';
import { LeaveSummary } from '@services/calendar/leave-summary.model';

@Injectable()
export class TeamViewService {
    constructor() {}

    public getSummaryForUsers(start: Date, team: number | null = null) {
        return of<TeamViewModel>({
            users: [{
                id: 0,
                name: 'test',
                total: 0,
                team: {
                    id: 0,
                    name: ''
                },
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
