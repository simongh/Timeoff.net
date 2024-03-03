import { FormBuilder } from "@angular/forms";
import { UserModel } from "../../models/user.model";
import { map, of } from "rxjs";
import { Injectable } from "@angular/core";
import { EmailModel } from "./email.model";
import { HttpClient } from "@angular/common/http";

interface QueryResult {
    pages: number;
    results: EmailModel[];
}

@Injectable()
export class EmailAuditService {
    public searchForm = this.fb.group({
        start: [null],
        end: [null],
        userId: [null as number | null]
    });

    public currentPage: number = 1;

    public totalPages: number = 0;
    
    constructor(
        private readonly fb: FormBuilder,
        private readonly client: HttpClient
    ) {}

    public getUsers() {
        return of([] as UserModel[]);
    }

    public search() {
        return this.client.get<QueryResult>('/api/audit/email',{
            params: {
                ...this.searchForm.value as any,
                page: this.currentPage
            }
        })
            .pipe(map((result) => {
                this.totalPages = result.pages;

                return result.results;
            }));
    }
}