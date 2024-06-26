import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { formatDate } from '@angular/common';

import { yearValidator } from '@components/validators';

import { PublicHolidayModel } from '@models/public-holiday.model';
import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';
import { format } from 'date-fns';

export interface holidayFormControls {
    id: FormControl<number | null>;
    name: FormControl<string | null>;
    date: FormControl<string | null>;
}
export type HolidayFormGroup = ReturnType<PublicHolidaysService['newForm']>;

@Injectable()
export class PublicHolidaysService {
    public holidays = this.fb.array<HolidayFormGroup>([]);

    public addForm!: HolidayFormGroup;

    constructor(
        private readonly client: HttpClient,
        private readonly fb: FormBuilder,
        private readonly currentUser: LoggedInUserService
    ) {
        this.setAddForm(0);
    }

    public get(year: number) {
        return this.client.get<PublicHolidayModel[]>(`/api/public-holidays/${year}`);
    }

    public setAddForm(year: number) {
        this.addForm = this.newForm(
            {
                id: null,
                name: '',
                date: null,
            },
            year
        );
    }

    public newForm(data: PublicHolidayModel, year: number) {
        return this.fb.group({
            id: data.id,
            name: [data.name, Validators.required],
            date: [data.date ? format(data.date, this.currentUser.dateFormat()) : null, yearValidator(year)],
        });
    }

    public addNew() {
        return this.client.post<void>('/api/public-holidays', {
            publicHolidays: [this.addForm.value],
        });
    }

    public update(model: PublicHolidayModel[]) {
        return this.client.put<void>('/api/public-holidays', {
            publicHolidays: model,
        });
    }

    public delete(id: number) {
        return this.client.delete(`/api/public-holidays/${id}`);
    }
}
