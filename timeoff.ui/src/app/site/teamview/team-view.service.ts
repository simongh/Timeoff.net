import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { TeamViewModel } from './team-view.model';
import { LeaveSummary } from '@services/calendar/leave-summary.model';

@Injectable()
export class TeamViewService {
    constructor() {}

    public getSummaryForUsers(start: Date, team: number | null = null) {
        return of<TeamViewModel>({
            holidays: [],
            users: [{
                id: 0,
                name: 'test',
                total: 0,
                leaveSummary: [] as LeaveSummary[],
                team: {
                    id: 0,
                    name: ''
                },
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
