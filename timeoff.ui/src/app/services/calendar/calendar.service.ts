import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { CalendarDayModel } from '@models/calendar-day.model';
import { CalendarModel } from './calendar.model';

@Injectable()
export class CalendarService {
    constructor(private readonly client: HttpClient) {}

    public publicHolidays(year: number) {
        return this.client.get<CalendarDayModel[]>(`/api/public-holidays/${year}`);
    }

    public get(year: number, userId: number | null = null) {
        const options = {
            params: new HttpParams().set('year', year),
        };

        if (!!userId) {
            options.params = options.params.append('user', userId);
        }

        return this.client.get<CalendarModel>(`/api/calendar`, options);
    }
}
