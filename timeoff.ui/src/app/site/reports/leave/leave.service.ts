import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { endOfMonth, formatISO, startOfMonth } from 'date-fns';
import { LeaveResultModel } from './leave-result.model';

type SearchFormGroup = ReturnType<LeaveService['createForm']>;

@Injectable()
export class LeaveService {
    public form: SearchFormGroup = this.createForm();

    constructor(private readonly client: HttpClient, private readonly fb: FormBuilder) {}

    private createForm() {
        const form = this.fb.group({
            leaveType: [null as number | null],
            team: [null as number | null],
            start: [formatISO(startOfMonth(new Date()), { representation: 'date' })],
            end: [formatISO(endOfMonth(new Date()), { representation: 'date' })],
        });

        return form;
    }

    public search(){
        const options = {
            params: new HttpParams()
                .set('start', this.form.value.start!)
                .set('end', this.form.value.end!)
        };

        if (!!this.form.value.leaveType){
            options.params = options.params.append('leave-type', this.form.value.leaveType)
        }

        if (!!this.form.value.team) {
            options.params.append('team',this.form.value.team)
        }

        return this.client.get<LeaveResultModel[]>('/api/absence-summary',options);
    }
}
