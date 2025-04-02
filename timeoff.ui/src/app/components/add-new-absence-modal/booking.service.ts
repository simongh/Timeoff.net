import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { formatISO } from 'date-fns';

import { datePart, dateString } from '@models/types';

@Injectable()
export class BookingService {
    readonly #client = inject(HttpClient);

    public readonly form =  inject(FormBuilder).group({
        employee: [null as number | null, Validators.required],
        leaveType: ['' as number | string, Validators.required],
        startPart: [datePart.wholeDay],
        start: [formatISO(new Date(), { representation: 'date' }) as dateString | null, Validators.required],
        endPart: [datePart.wholeDay],
        end: [formatISO(new Date(), { representation: 'date' }) as dateString | null, Validators.required],
        comment: [null as string | null],
    });

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
        return this.#client.post<void>('/api/absences', this.form.value);
    }
}
