import { CommonModule } from '@angular/common';
import { Component, computed, inject, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { eachDayOfInterval, endOfMonth, formatDate } from 'date-fns';

import { TeamModel } from '@services/company/team.model';
import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';

import { TeamViewModel } from './team-view.model';
import { RowModel } from './row.model';
import { DayModel } from './day.model';

@Component({
    standalone: true,
    templateUrl: 'month-view.component.html',
    styleUrl: 'month-view.component.scss',
    selector: 'month-view',
    imports: [CommonModule, RouterLink],
})
export class MonthViewComponent {
    public readonly team = input<TeamModel | null>(null);

    public readonly selectedDate = input.required<Date>();

    public readonly selectedTeam = input.required<number | null>();

    public readonly teams = input.required<TeamModel[]>();

    public readonly results = input.required<TeamViewModel>();

    protected readonly isAdmin = inject(LoggedInUserService).isAdmin;

    protected readonly selectedTeamName = computed(() => this.teams().find((t) => t.id === this.selectedTeam())?.name);

    protected readonly days = computed(() => {
        return eachDayOfInterval({
            start: this.selectedDate(),
            end: endOfMonth(this.selectedDate()),
        });
    });

    protected readonly rows = computed(()=> {
        return this.results().users.map(
            (u) =>
                ({
                    name: u.name,
                    id: u.id,
                    total: u.total,
                    summary: `In ${formatDate(this.selectedDate(), 'MMMM, yyyy')} ${u.name} used ${
                        u.total
                    } days from allowance`,
                    days: this.days().map((d) => new DayModel(d, u)),
                } as RowModel)
        );
    });
}
