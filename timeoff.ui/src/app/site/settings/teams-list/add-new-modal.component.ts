import { CommonModule } from "@angular/common";
import { Component, DestroyRef, EventEmitter, OnInit } from "@angular/core";
import { TeamsService } from "../../../services/teams/teams.service";
import { ReactiveFormsModule } from "@angular/forms";
import { ValidatorMessageComponent } from "../../../components/validator-message/validator-message.component";
import { TeamModel } from "../../../services/teams/team.model";
import { ErrorsService } from "../../../services/errors/errors.service";
import { FlashModel, isSuccess } from "../../../components/flash/flash.model";
import { UserModel } from "../../../models/user.model";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";

@Component({
    standalone: true,
    templateUrl: 'add-new-modal.component.html',
    selector: 'add-new-modal',
    imports: [CommonModule,ReactiveFormsModule,ValidatorMessageComponent]
})
export class AddNewModalComponent implements OnInit {
    public allowance: number[] = [];

    public users: UserModel[] = [];

    public get form() {
        return this.teamsSvc.form;
    }

    public messages = new FlashModel();

    constructor(
        private readonly teamsSvc: TeamsService,
        private destroyed: DestroyRef,
    ) {}

    public ngOnInit(): void {
        for (let index = 0; index <= 50; index += 0.5) {
            this.allowance.push(index);
        }

        this.teamsSvc.getUsers()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe((data) => this.users = data);
    }

    public add() {
        this.messages = isSuccess('New team added');
        this.form.markAllAsTouched();

        if (this.form.invalid) {
            return;
        }

        // this.newTeam.emit({
        //     name: this.form.value.name,
        //     allowance: this.form.value.allowance,
        //     includePublicHolidays: this.form.value.includePublicHolidays,
        //     isAccruedAllowance: this.form.value.accruedAllowance,
        //     manager: this.form.value.manager
        // } as TeamModel)
    }
}