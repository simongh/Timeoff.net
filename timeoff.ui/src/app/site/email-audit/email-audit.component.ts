import { Component, DestroyRef, OnInit, numberAttribute, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { injectQueryParams } from 'ngxtension/inject-query-params';
import { computedAsync } from 'ngxtension/computed-async';

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
    protected get form() {
        return this.searchSvc.searchForm;
    }

    protected readonly dateFormat = this.currentUser.dateFormat;

    protected readonly users = computedAsync(() => this.companySvc.getUsers(), { initialValue: [] });

    protected readonly emails = signal<EmailModel[]>([]);

    protected readonly searching = signal(false);

    protected readonly currentPage = injectQueryParams((p) => numberAttribute(p['page'] ?? 1));

    protected readonly user = injectQueryParams((p) => (!!p['user'] ? numberAttribute(p['user']) : null));

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
        this.searchSvc.currentPage = this.currentPage();

        const u = this.user();
        this.searchSvc.searchForm.controls.user.setValue(u);

        this.find();
    }

    public search() {
        this.searching.set(true);

        this.searchSvc.currentPage = 1;
        this.find();
    }

    public reset() {
        this.form.reset({
            user: null,
            start: '',
            end: '',
        });
        this.search();
    }

    public searchByUser(id: number) {
        this.form.controls.user.setValue(id);

        this.search();
    }

    private find() {
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
