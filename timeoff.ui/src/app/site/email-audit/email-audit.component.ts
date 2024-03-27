import { Component, DestroyRef, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { switchMap } from 'rxjs';

import { FlashComponent } from '@components/flash/flash.component';

import { MessagesService } from '@services/messages/messages.service';
import { UserModel } from '@services/company/user.model';
import { CompanyService } from '@services/company/company.service';

import { PagerComponent } from './pager.component';
import { EmailModel } from './email.model';
import { EmailAuditService } from './email-audit.service';

@Component({
    selector: 'email-audit',
    standalone: true,
    providers: [EmailAuditService, CompanyService],
    templateUrl: './email-audit.component.html',
    styleUrl: './email-audit.component.scss',
    imports: [ReactiveFormsModule, CommonModule, PagerComponent, FlashComponent],
})
export class EmailAuditComponent implements OnInit {
    public get form() {
        return this.searchSvc.searchForm;
    }

    public dateFormat = 'yyyy-mm-dd';

    public users: UserModel[] = [];

    public emails: EmailModel[] = [];

    public searching = false;

    public get currentPage() {
        return this.searchSvc.currentPage;
    }

    public get totalPages() {
        return this.searchSvc.totalPages;
    }

    constructor(
        private readonly searchSvc: EmailAuditService,
        private readonly msgsSvc: MessagesService,
        private readonly companySvc: CompanyService,
        private readonly destroyed: DestroyRef,
        private readonly route: ActivatedRoute
    ) {}

    public ngOnInit(): void {
        this.companySvc
            .getUsers()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe((data) => {
                this.users = data;
            });

        this.find();
    }

    public search() {
        this.searchSvc.currentPage = 1;
        this.find();
    }

    public reset() {
        this.form.reset({
            userId: '',
            start: '',
            end: '',
        });
        this.search();
    }

    public searchByUser(userId: number) {
        this.form.controls.userId.setValue(userId.toString());

        this.search();
    }

    private find() {
        this.searching = true;

        this.route.queryParamMap
            .pipe(
                takeUntilDestroyed(this.destroyed),
                switchMap((p) => {
                    if (p.has('page')) {
                        this.searchSvc.currentPage = Number.parseInt(p.get('page')!);
                    } else {
                        this.searchSvc.currentPage = 1;
                    }

                    if (p.has('user')) {
                        this.form.controls.userId.setValue(p.get('user')!);
                    }

                    return this.searchSvc.search();
                })
            )
            .subscribe({
                next: (data) => {
                    this.emails = data;
                    this.searching = false;
                },
                error: () => {
                    this.msgsSvc.isError('Unable to retrieve emails');
                    this.emails = [];
                },
            });
    }
}
