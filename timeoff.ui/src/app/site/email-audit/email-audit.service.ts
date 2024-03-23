import { FormBuilder } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { UserModel } from '../../services/company/user.model';
import { EmailModel } from './email.model';
import { dateString } from '../../components/types';

interface QueryResult {
    pages: number;
    results: EmailModel[];
}

@Injectable()
export class EmailAuditService {
    public searchForm = this.fb.group({
        start: ['' as dateString | null],
        end: ['' as dateString | null],
        userId: [''],
    });

    public currentPage: number = 1;

    public totalPages: number = 0;

    constructor(private readonly fb: FormBuilder, private readonly client: HttpClient) {}

    public getUsers() {
        return this.client.get<UserModel[]>('/api/teams/users');
    }

    public search() {
        return this.client
            .get<QueryResult>('/api/audit/email', {
                params: {
                    ...this.searchForm.value,
                    page: this.currentPage,
                } as any,
            })
            .pipe(
                map((result) => {
                    this.totalPages = result.pages;

                    return result.results;
                })
            );
    }
}
