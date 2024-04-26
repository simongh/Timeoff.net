import { CommonModule } from '@angular/common';
import { Component, DestroyRef, EventEmitter, Output, signal } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { HttpErrorResponse } from '@angular/common/http';

import { ValidatorMessageComponent } from '@components/validator-message/validator-message.component';
import { UserListComponent } from '@components/user-select/user-select.component';
import { getAllowances } from '@models/allowances';

import { MessagesService } from '@services/messages/messages.service';

import { TeamsService } from '../teams.service';

@Component({
    standalone: true,
    templateUrl: 'add-new-modal.component.html',
    selector: 'add-new-modal',
    imports: [CommonModule, ReactiveFormsModule, ValidatorMessageComponent, UserListComponent],
})
export class AddNewModalComponent {
    protected readonly allowance = getAllowances();

    protected get form() {
        return this.teamsSvc.form;
    }

    @Output()
    public readonly added = new EventEmitter();

    protected readonly submitting = signal(false);

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
        this.submitting.set(true);
        this.teamsSvc
            .create()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.msgSvc.isSuccess('New team added');
                    this.added.emit();
                    this.form.reset();
                    this.submitting.set(false);
                },
            });
    }

    public cancel() {
        this.teamsSvc.reset();
    }
}
