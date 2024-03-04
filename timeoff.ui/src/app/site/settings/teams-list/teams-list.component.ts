import { Component, DestroyRef, OnInit } from "@angular/core";
import { FlashComponent } from "../../../components/flash/flash.component";
import { TeamsService } from "../../../services/teams/teams.service";
import { TeamModel } from "../../../services/teams/team.model";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { CommonModule } from "@angular/common";
import { RouterLink } from "@angular/router";
import { YesPipe } from "../../../components/yes.pipe";

@Component({
    standalone: true,
    templateUrl: 'teams-list.component.html',
    imports: [FlashComponent, CommonModule, RouterLink, YesPipe],
    providers: [TeamsService]
})
export class TeamsListComponent implements OnInit {
    public teams!: TeamModel[];

    constructor(
        private readonly teamSvc: TeamsService,
        private destroyed: DestroyRef){}
    
    public ngOnInit(): void {
        this.teamSvc.getTeams()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe((t)=>{
                this.teams = t;
            });
    }
}