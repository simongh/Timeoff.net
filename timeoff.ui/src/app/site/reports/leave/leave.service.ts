import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { endOfMonth, formatISO, startOfMonth } from 'date-fns';

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
}
