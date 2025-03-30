import { Component, inject, input } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { derivedAsync } from 'ngxtension/derived-async';

import { CompanyService } from '@services/company/company.service';

@Component({
    selector: 'user-select',
    imports: [CommonModule, ReactiveFormsModule],
    templateUrl: './user-select.component.html',
    styleUrl: './user-select.component.scss',
    providers: [CompanyService]
})
export class UserListComponent {
    private readonly companySvc = inject(CompanyService);

    protected readonly users = derivedAsync(() => this.companySvc.getUsers(), { initialValue: [] });

    public readonly for = input('');

    public readonly control = input.required<FormControl<number | null>>();
}
