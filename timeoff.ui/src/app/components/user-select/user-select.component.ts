import { Component, inject, input } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

import { CompanyService } from '@services/company/company.service';
import { toSignal } from '@angular/core/rxjs-interop';

@Component({
    selector: 'user-select',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule],
    templateUrl: './user-select.component.html',
    styleUrl: './user-select.component.scss',
    providers: [CompanyService],
})
export class UserListComponent {
    readonly #companySvc = inject(CompanyService);

    protected readonly users = toSignal(this.#companySvc.getUsers(), { initialValue: [] });

    public readonly for = input('');

    public readonly control = input.required<FormControl<number | null>>();
}
