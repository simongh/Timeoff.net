import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { formatISO } from 'date-fns';

import { datePart, dateString } from '@models/types';

type BookingFormGroup = ReturnType<BookingService['createForm']>;

@Injectable()
export class BookingService {
    public readonly form: BookingFormGroup = this.createForm();

    constructor(private readonly fb: FormBuilder, private readonly client: HttpClient) {}

    public reset() {
        this.form.reset({
            employee: null,
            leaveType: '',
            startPart: datePart.wholeDay,
            start: formatISO(new Date(), { representation: 'date' }),
            endPart: datePart.wholeDay,
            end: formatISO(new Date(), { representation: 'date' }),
            comment: null,
        });
    }

    public add() {
        return this.client.post<void>('/api/absences', this.form.value);
    }

    private createForm() {
        return this.fb.group({
            employee: [null as number | null, Validators.required],
            leaveType: ['' as number | string, Validators.required],
            startPart: [datePart.wholeDay],
            start: [formatISO(new Date(), { representation: 'date' }) as dateString | null, Validators.required],
            endPart: [datePart.wholeDay],
            end: [formatISO(new Date(), { representation: 'date' }) as dateString | null, Validators.required],
            comment: [null as string | null],
        });
    }
}
