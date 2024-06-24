import { Injectable } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { endOfMonth, formatDate, formatISO, parseISO, startOfMonth } from 'date-fns';

import { dateString } from '@models/types';
import { HttpClient, HttpParams } from '@angular/common/http';
import { AllowanceModel } from './allowance.model';

type SearchFormGroup = ReturnType<AllowanceUsageService['createForm']>;

@Injectable()
export class AllowanceUsageService {
    public form: SearchFormGroup;

    public get start() {
        return parseISO(this.form.value.start!);
    }

    public get end() {
        return parseISO(this.form.value.end!);
    }

    constructor(private readonly fb: FormBuilder, private readonly client: HttpClient) {
        this.form = this.createForm();
    }

    public getResults() {        
        const options = {
            params: new HttpParams()
                .set('start', formatISO(startOfMonth(this.start), { representation: 'date' }))
                .set('end', formatISO(endOfMonth(this.end), { representation: 'date' })),
        };

        if (this.form.value.team) {
            options.params = options.params.append('team', this.form.value.team);
        }

        return this.client.get<AllowanceModel[]>('/api/reports/allowance-usage', options);
    }

    private createForm() {
        return this.fb.group({
            team: [null as number | null],
            start: [formatDate(new Date(), 'yyyy-MM') as dateString, Validators.required],
            end: [formatDate(new Date(), 'yyyy-MM') as dateString, Validators.required],
        });
    }
}
