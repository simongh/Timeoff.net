import { Component, DestroyRef, OnInit, numberAttribute, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { switchMap } from 'rxjs';
import { injectQueryParams } from 'ngxtension/inject-query-params';

import { FlashComponent } from '@components/flash/flash.component';
import { DatePickerDirective } from '@components/date-picker.directive';

import { MessagesService } from '@services/messages/messages.service';
import { UserModel } from '@services/company/user.model';
import { CompanyService } from '@services/company/company.service';

import { PagerComponent } from './pager.component';
import { EmailModel } from './email.model';
import { EmailAuditService } from './email-audit.service';
import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';

@Component({
    selector: 'email-audit',
    standalone: true,
    providers: [EmailAuditService, CompanyService],
    templateUrl: './email-audit.component.html',
    styleUrl: './email-audit.component.scss',
    imports: [ReactiveFormsModule, CommonModule, PagerComponent, FlashComponent, DatePickerDirective],
})
export class EmailAuditComponent implements OnInit {
    public get form() {
        return this.searchSvc.searchForm;
    }

    protected readonly dateFormat = this.currentUser.dateFormat;

    protected readonly users = signal<UserModel[]>([]);

    protected readonly emails = signal<EmailModel[]>([]);

    protected readonly searching = signal(false);

    protected readonly currentPage = injectQueryParams((p) => numberAttribute(p['page'] ?? 1));

    protected readonly user = injectQueryParams((p) => numberAttribute(p['user']));

    protected readonly totalPages = signal(0);

    constructor(
        private readonly searchSvc: EmailAuditService,
        private readonly msgsSvc: MessagesService,
        private readonly companySvc: CompanyService,
        private readonly destroyed: DestroyRef,
        private readonly route: ActivatedRoute,
        private readonly currentUser: LoggedInUserService
    ) {}

    public ngOnInit(): void {
        this.companySvc
            .getUsers()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe((data) => {
                this.users.set(data);
            });

        this.find();
    }

    public search() {
        this.searchSvc.currentPage = 1;
        this.find();
    }

    public reset() {
        this.form.reset({
            userId: null,
            start: '',
            end: '',
        });
        this.search();
    }

    public searchByUser(userId: number) {
        this.form.controls.userId.setValue(userId);

        this.search();
    }

    private find() {
        this.searching.set(true);

        this.searchSvc.currentPage = this.currentPage();
        this.searchSvc.searchForm.controls.userId.setValue(this.user());

        this.searchSvc
            .search()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: (data) => {
                    this.emails.set(data);
                    this.searching.set(false);
                    this.totalPages.set(this.searchSvc.totalPages);
                },
                error: () => {
                    this.msgsSvc.isError('Unable to retrieve emails');
                    this.emails.set([]);
                },
            });
    }
}
