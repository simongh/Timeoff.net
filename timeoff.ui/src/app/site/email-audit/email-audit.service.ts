import { FormBuilder } from '@angular/forms';
import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map } from 'rxjs';

import { EmailModel } from './email.model';
import { dateString } from '@models/types';

interface QueryResult {
    pages: number;
    results: EmailModel[];
}

export type EmailSearchGroup = EmailAuditService['searchForm'];

@Injectable()
export class EmailAuditService {
    readonly #client = inject(HttpClient);

    public searchForm = inject(FormBuilder).group({
        start: ['' as dateString | null],
        end: ['' as dateString | null],
        user: [null as number | null],
    });

    public currentPage: number = 1;

    public totalPages: number = 0;

    public search() {
        const options = {
            params: new HttpParams()
                .set('page', this.currentPage),
        };

        if (!!this.searchForm.value.start) {
            options.params = options.params.append('start', this.searchForm.value.start);
        }

        if (!!this.searchForm.value.end) {
            options.params = options.params.append('end', this.searchForm.value.end);
        }

        const u = this.searchForm.controls.user.value;
        if (!!u && u.toString() != 'null') {
            options.params = options.params.append('user', u);
        }

        return this.#client.get<QueryResult>('/api/audit/email', options).pipe(
            map((result) => {
                this.totalPages = result.pages;

                return result.results;
            })
        );
    }
}
