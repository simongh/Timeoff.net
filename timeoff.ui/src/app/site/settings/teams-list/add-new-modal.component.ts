import { CommonModule } from "@angular/common";
import { Component, DestroyRef, EventEmitter, OnInit, Output } from "@angular/core";
import { TeamsService } from "../../../services/teams/teams.service";
import { ReactiveFormsModule } from "@angular/forms";
import { ValidatorMessageComponent } from "../../../components/validator-message/validator-message.component";
import { FlashModel, hasErrors, isError, isSuccess } from "../../../components/flash/flash.model";
import { UserModel } from "../../../models/user.model";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { HttpErrorResponse } from "@angular/common/http";

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

    @Output()
    public added = new EventEmitter();

    public submitting = false;

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
        this.form.markAllAsTouched();

        if (this.form.invalid) {
            return;
        }
        this.submitting = true;
        this.teamsSvc.create()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.messages = isSuccess('New team added');
                    this.added.emit();
                    this.form.reset();
                    this.submitting = false;
                },
                error: (e: HttpErrorResponse) => {
                    if (e.status == 400) {
                        this.messages = hasErrors(e.error.errors);
                    } else {
                        this.messages = isError('Unable to add new team');
                    }

                    this.submitting = false;
                }
            })
    }

    public cancel() {
        this.teamsSvc.reset();
    }
}