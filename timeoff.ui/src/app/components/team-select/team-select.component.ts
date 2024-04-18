import { Component, DestroyRef, effect, input, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

import { CompanyService } from '@services/company/company.service';
import { TeamModel } from '@services/company/team.model';

@Component({
    selector: 'team-select',
    standalone: true,
    imports: [ReactiveFormsModule, CommonModule],
    templateUrl: './team-select.component.html',
    styleUrl: './team-select.component.scss',
    providers: [CompanyService],
})
export class TeamSelectComponent {
    protected readonly teams = signal<TeamModel[]>([]);

    public control = input.required<FormControl<number | null>>();

    public for = input('');

    public allowAll = input(false);

    constructor(private destroyed: DestroyRef, private readonly companySvc: CompanyService) {
        effect(() => {
            this.companySvc
                .getTeams()
                .pipe(takeUntilDestroyed(this.destroyed))
                .subscribe((teams) => this.teams.set(teams));
        });
    }
}
