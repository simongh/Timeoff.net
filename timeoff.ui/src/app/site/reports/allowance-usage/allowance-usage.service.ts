import { Injectable } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { formatDate } from 'date-fns';

import { dateString } from '@components/types';

type SearchFormGroup = ReturnType<AllowanceUsageService['createForm']>

@Injectable()
export class AllowanceUsageService {
    public form: SearchFormGroup;

    constructor(private readonly fb: FormBuilder) {
      this.form = this.createForm();
    }

    private createForm() {
      return this.fb.group({
        team: [null as number | null],
        start: [formatDate(new Date(),'yyyy-MM') as dateString | null],
        end: [formatDate(new Date(),'yyyy-MM') as dateString | null]
      })
    }
}
