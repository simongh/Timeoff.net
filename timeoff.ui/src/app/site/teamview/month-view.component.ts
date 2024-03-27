import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { eachDayOfInterval, endOfMonth, formatDate } from 'date-fns';

import { TeamModel } from '@services/company/team.model';
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
    @Input()
    public team: TeamModel | null = null;

    @Input()
    public selectedDate!: Date;

    @Input()
    public selectedTeam!: number | null;

    @Input()
    public teams!: TeamModel[];

    @Input()
    public results!: TeamViewModel;

    public isAdmin = true;

    public get selectedTeamName() {
        return this.teams.find((t) => t.id === this.selectedTeam)?.name;
    }

    public get days() {
        return eachDayOfInterval({
            start: this.selectedDate,
            end: endOfMonth(this.selectedDate),
        });
    }

    public get rows() {
        return this.results.users.map(
            (u) =>
                ({
                    name: u.name,
                    id: u.id,
                    total: u.total,
                    summary: `In ${formatDate(this.selectedDate,'MMMM, yyyy')} ${u.name} used ${u.total} days from allowance`,
                    days: this.days.map((d) => new DayModel(d, u, this.results.holidays)),
                } as RowModel)
        );
    }
}
