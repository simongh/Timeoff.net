import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { eachDayOfInterval, endOfMonth } from 'date-fns';

import { TeamModel } from '@services/company/team.model';

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

    public get selectedTeamName() {
        return this.teams.find((t) => t.id === this.selectedTeam)?.name;
    }

    public get days() {
        return eachDayOfInterval({
            start: this.selectedDate,
            end: endOfMonth(this.selectedDate),
        });
    }
}
