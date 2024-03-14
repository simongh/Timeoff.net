import { Component, DestroyRef, OnInit, ViewChild } from "@angular/core";
import { FlashComponent } from "../../../components/flash/flash.component";
import { TeamsService } from "../../../services/teams/teams.service";
import { TeamModel } from "../../../services/teams/team.model";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { CommonModule } from "@angular/common";
import { RouterLink } from "@angular/router";
import { YesPipe } from "../../../components/yes.pipe";
import { AddNewModalComponent } from "./add-new-modal.component";
import { FlashModel } from "../../../components/flash/flash.model";
import { MessagesService } from "../../../services/messages/messages.service";

@Component({
    standalone: true,
    templateUrl: 'teams-list.component.html',
    providers: [TeamsService],
    imports: [FlashComponent, CommonModule, RouterLink, YesPipe, AddNewModalComponent]
})
export class TeamsListComponent implements OnInit {
    public teams: TeamModel[] = [];

    public messages = new FlashModel();

    @ViewChild(AddNewModalComponent)
    private addNewModel!: AddNewModalComponent

    constructor(
        private readonly teamSvc: TeamsService,
        private readonly messagesSvc: MessagesService,
        private destroyed: DestroyRef
    ){}
    
    public ngOnInit(): void {
        this.refresh();
        console.log(history.state.message);

        this.messages = this.messagesSvc.getMessages();
    }

    public added()
    {
        this.messages = this.addNewModel.messages;
        this.refresh();
    }

    private refresh() {

        this.teamSvc.getTeams()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe((t)=>{
                this.teams = t;
            });
    }
}