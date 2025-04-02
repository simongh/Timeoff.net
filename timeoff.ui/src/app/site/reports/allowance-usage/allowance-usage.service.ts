import { inject, Injectable } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { HttpClient, HttpParams } from '@angular/common/http';
import { endOfMonth, formatDate, formatISO, parseISO, startOfMonth } from 'date-fns';

import { dateString } from '@models/types';
import { AllowanceModel } from './allowance.model';

export type SearchFormGroup = AllowanceUsageService['form'];

@Injectable()
export class AllowanceUsageService {
    readonly #client = inject(HttpClient);

    public readonly form = inject(FormBuilder).group({
        team: [null as number | null],
        start: [formatDate(new Date(), 'yyyy-MM') as dateString, Validators.required],
        end: [formatDate(new Date(), 'yyyy-MM') as dateString, Validators.required],
    });

    public get start() {
        return parseISO(this.form.value.start!);
    }

    public get end() {
        return parseISO(this.form.value.end!);
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

        return this.#client.get<AllowanceModel[]>('/api/reports/allowance-usage', options);
    }
}
