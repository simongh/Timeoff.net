import { NgFor } from '@angular/common';
import { Component, inject, input } from '@angular/core';
import { derivedAsync } from 'ngxtension/derived-async';

import { CompanyService } from '@services/company/company.service';
import { DateInputDirective } from '@components/date-input.directive';

import { EmailSearchGroup } from './email-audit.service';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
    templateUrl: 'filter.component.html',
    selector: 'filter',
    imports: [ReactiveFormsModule, NgFor, DateInputDirective],
})
export class FilterComponent {
    readonly #companySvc = inject(CompanyService);

    public form = input.required<EmailSearchGroup>();

    protected readonly users = derivedAsync(() => this.#companySvc.getUsers(), { initialValue: [] });
}
