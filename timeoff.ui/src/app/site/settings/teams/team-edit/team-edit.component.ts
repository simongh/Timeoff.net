import { Component, DestroyRef, OnInit, numberAttribute } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { injectParams } from 'ngxtension/inject-params';

import { FlashComponent } from '@components/flash/flash.component';
import { ValidatorMessageComponent } from '@components/validator-message/validator-message.component';
import { UserListComponent } from '@components/user-select/user-select.component';
import { getAllowances } from '@models/allowances';

import { MessagesService } from '@services/messages/messages.service';

import { TeamsService } from '../teams.service';

@Component({
    selector: 'team-edit',
    standalone: true,
    providers: [TeamsService],
    templateUrl: './team-edit.component.html',
    styleUrl: './team-edit.component.scss',
    imports: [
        FlashComponent,
        RouterLink,
        ReactiveFormsModule,
        ValidatorMessageComponent,
        CommonModule,
        UserListComponent,
    ],
})
export class TeamEditComponent implements OnInit {
    protected get name() {
        return this.editForm.controls.name.value;
    }

    protected get editForm() {
        return this.teamSvc.form;
    }

    protected readonly allowances = getAllowances();

    protected readonly id = injectParams((p) => numberAttribute(p['id']));

    constructor(
        private destroyed: DestroyRef,
        private readonly router: Router,
        private readonly teamSvc: TeamsService,
        private readonly messagesSvc: MessagesService
    ) {}

    public ngOnInit(): void {
        this.teamSvc
            .get(this.id())
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe((data) => {
                this.editForm.setValue({
                    name: data.name,
                    allowance: data.allowance,
                    includePublicHolidays: data.includePublicHolidays,
                    isAccruedAllowance: data.isAccruedAllowance,
                    manager: data.manager.id,
                });
            });
    }

    public save() {
        this.editForm.markAllAsTouched();

        if (this.editForm.invalid) {
            return;
        }

        const f = this.editForm.value;
        this.teamSvc
            .update(this.id())
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => this.messagesSvc.isSuccess(`Team ${this.name} was updated`),
            });
    }

    public delete() {
        this.teamSvc
            .delete(this.id())
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.messagesSvc.isSuccess(`Team ${this.name} was deleted`, true);
                    this.router.navigate(['settings', 'teams']);
                },
            });
    }
}
