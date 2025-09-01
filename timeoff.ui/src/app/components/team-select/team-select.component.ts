import { Component, inject, input } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

import { CompanyService } from '@services/company/company.service';
import { toSignal } from '@angular/core/rxjs-interop';

@Component({
    selector: 'team-select',
    imports: [ReactiveFormsModule],
    templateUrl: './team-select.component.html',
    styleUrl: './team-select.component.scss',
    providers: [CompanyService]
})
export class TeamSelectComponent {
    readonly #companySvc = inject(CompanyService);

    protected readonly teams = toSignal(this.#companySvc.getTeams(), { initialValue: [] });

    public readonly control = input.required<FormControl<number | null>>();

    public readonly for = input('');

    public readonly allowAll = input(false);
}