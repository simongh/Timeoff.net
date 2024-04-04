import { Component, DestroyRef, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { HttpErrorResponse } from '@angular/common/http';
import { switchMap } from 'rxjs';

import { FlashComponent } from '@components/flash/flash.component';
import { ValidatorMessageComponent } from '@components/validator-message/validator-message.component';
import { UserListComponent } from '@components/user-select/user-select.component';
import { getAllowances } from '@components/allowances';

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
    public get name() {
        return this.editForm.controls.name.value;
    }

    public get editForm() {
        return this.teamSvc.form;
    }

    public allowances = getAllowances();

    public id!: number;

    constructor(
        private readonly route: ActivatedRoute,
        private destroyed: DestroyRef,
        private readonly router: Router,
        private readonly teamSvc: TeamsService,
        private readonly messagesSvc: MessagesService
    ) {}

    public ngOnInit(): void {
        this.route.paramMap
            .pipe(
                takeUntilDestroyed(this.destroyed),
                switchMap((p) => {
                    if (p.has('id')) {
                        this.id = Number.parseInt(p.get('id')!);
                    } else {
                        throw new Error('No team id specified');
                    }

                    return this.teamSvc.get(this.id);
                })
            )
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
            .update(this.id)
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => this.messagesSvc.isSuccess(`Team ${this.name} was updated`),
                error: (e: HttpErrorResponse) => {
                    if (e.status == 400) {
                        this.messagesSvc.hasErrors(e.error.errors);
                    } else {
                        this.messagesSvc.isError(`Unable to update team ${this.name}`);
                    }
                },
            });
    }

    public delete() {
        this.teamSvc
            .delete(this.id)
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.messagesSvc.isSuccess(`Team ${this.name} was deleted`, true);
                    this.router.navigate(['settings', 'teams']);
                },
                error: (e: HttpErrorResponse) => {
                    if (e.status == 400) {
                        this.messagesSvc.hasErrors(e.error.errors);
                    } else {
                        this.messagesSvc.isError('Unable to delete the team');
                    }
                },
            });
    }
}
