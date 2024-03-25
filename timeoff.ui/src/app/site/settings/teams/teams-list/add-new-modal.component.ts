import { CommonModule } from '@angular/common';
import { Component, DestroyRef, EventEmitter, Output } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { HttpErrorResponse } from '@angular/common/http';

import { ValidatorMessageComponent } from '@components/validator-message/validator-message.component';
import { UserListComponent } from '@components/user-select/user-select.component';
import { getAllowances } from '@components/allowances';

import { UserModel } from '@services/company/user.model';
import { MessagesService } from '@services/messages/messages.service';

import { TeamsService } from '../teams.service';

@Component({
    standalone: true,
    templateUrl: 'add-new-modal.component.html',
    selector: 'add-new-modal',
    imports: [CommonModule, ReactiveFormsModule, ValidatorMessageComponent, UserListComponent],
})
export class AddNewModalComponent {
    public allowance: number[] = getAllowances();

    public users: UserModel[] = [];

    public get form() {
        return this.teamsSvc.form;
    }

    @Output()
    public added = new EventEmitter();

    public submitting = false;

    constructor(
        private readonly teamsSvc: TeamsService,
        private readonly msgSvc: MessagesService,
        private destroyed: DestroyRef
    ) {}

    public add() {
        this.form.markAllAsTouched();

        if (this.form.invalid) {
            return;
        }
        this.submitting = true;
        this.teamsSvc
            .create()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.msgSvc.isSuccess('New team added');
                    this.added.emit();
                    this.form.reset();
                    this.submitting = false;
                },
                error: (e: HttpErrorResponse) => {
                    if (e.status == 400) {
                        this.msgSvc.hasErrors(e.error.errors);
                    } else {
                        this.msgSvc.isError('Unable to add new team');
                    }

                    this.submitting = false;
                },
            });
    }

    public cancel() {
        this.teamsSvc.reset();
    }
}
