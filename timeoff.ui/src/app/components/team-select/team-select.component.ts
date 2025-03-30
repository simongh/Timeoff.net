import { Component, inject, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { derivedAsync } from 'ngxtension/derived-async';

import { CompanyService } from '@services/company/company.service';

@Component({
    selector: 'team-select',
    imports: [ReactiveFormsModule, CommonModule],
    templateUrl: './team-select.component.html',
    styleUrl: './team-select.component.scss',
    providers: [CompanyService]
})
export class TeamSelectComponent {
    private readonly companySvc = inject(CompanyService); 

    protected readonly teams = derivedAsync(() => this.companySvc.getTeams(), { initialValue: [] });

    public readonly control = input.required<FormControl<number | null>>();

    public readonly for = input('');

    public readonly allowAll = input(false);
}
