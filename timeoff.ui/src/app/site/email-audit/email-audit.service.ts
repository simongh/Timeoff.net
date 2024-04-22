import { FormBuilder } from '@angular/forms';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, numberAttribute } from '@angular/core';
import { map } from 'rxjs';

import { EmailModel } from './email.model';
import { dateString } from '@models/types';

interface QueryResult {
    pages: number;
    results: EmailModel[];
}

@Injectable()
export class EmailAuditService {
    public searchForm = this.fb.group({
        start: ['' as dateString | null],
        end: ['' as dateString | null],
        user: [null as number | null],
    });

    public currentPage: number = 1;

    public totalPages: number = 0;

    constructor(private readonly fb: FormBuilder, private readonly client: HttpClient) {}

    public search() {
        const options = {
            params: new HttpParams()
                .set('page', this.currentPage)
                .set('start', this.searchForm.value.start!)
                .set('end', this.searchForm.value.end!),
        };

        const u = this.searchForm.controls.user.value;
        if (!!u && u.toString() != 'null') {
            options.params = options.params.append('user', u);
        }

        return this.client.get<QueryResult>('/api/audit/email', options).pipe(
            map((result) => {
                this.totalPages = result.pages;

                return result.results;
            })
        );
    }
}
