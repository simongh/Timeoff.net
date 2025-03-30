import { NgFor } from '@angular/common';
import { Component, inject } from '@angular/core';
import { derivedAsync } from 'ngxtension/derived-async';

import { CompanyService } from '@services/company/company.service';
import { DateInputDirective } from '@components/date-input.directive';

import { EmailAuditService } from './email-audit.service';

@Component({
    templateUrl: 'filter.component.html',
    selector: 'filter',
    imports: [NgFor, DateInputDirective],
})
export class FilterComponent {
    readonly #companySvc = inject(CompanyService);

    readonly #searchSvc = inject(EmailAuditService);

    protected get form() {
        return this.#searchSvc.searchForm;
    }

    protected readonly users = derivedAsync(() => this.#companySvc.getUsers(), { initialValue: [] });
}
