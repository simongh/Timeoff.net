import { Component, DestroyRef, OnInit, ViewChild } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FlashComponent } from '../../../components/flash/flash.component';
import { TeamsService } from '../../../services/teams/teams.service';
import { TeamModel } from '../../../services/teams/team.model';
import { YesPipe } from '../../../components/yes.pipe';
import { AddNewModalComponent } from './add-new-modal.component';

@Component({
    standalone: true,
    templateUrl: 'teams-list.component.html',
    providers: [TeamsService],
    imports: [FlashComponent, CommonModule, RouterLink, YesPipe, AddNewModalComponent],
})
export class TeamsListComponent implements OnInit {
    public teams: TeamModel[] = [];

    constructor(private readonly teamSvc: TeamsService, private destroyed: DestroyRef) {}

    public ngOnInit(): void {
        this.refresh();
    }

    public added() {
        this.refresh();
    }

    private refresh() {
        this.teamSvc
            .getTeams()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe((t) => {
                this.teams = t;
            });
    }
}
