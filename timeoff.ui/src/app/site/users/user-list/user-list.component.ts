import { Component, DestroyRef, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { HttpErrorResponse } from '@angular/common/http';
import { switchMap } from 'rxjs';

import { FlashComponent } from '@components/flash/flash.component';
import { YesPipe } from '@components/yes.pipe';

import { TeamModel } from '@services/company/team.model';
import { MessagesService } from '@services/messages/messages.service';
import { CompanyService } from '@services/company/company.service';

import { UsersService } from '../users.service';
import { UserListModel } from '../user-list.model';

@Component({
    selector: 'user-list',
    standalone: true,
    templateUrl: './user-list.component.html',
    styleUrl: './user-list.component.scss',
    providers: [UsersService, CompanyService],
    imports: [FlashComponent, RouterLink, CommonModule, YesPipe],
})
export class UserListComponent implements OnInit {
    public name: string = '';

    public teams!: TeamModel[];

    public team: number | null = null;

    public users!: UserListModel[];

    constructor(
        private readonly route: ActivatedRoute,
        private destroyed: DestroyRef,
        private usersSvc: UsersService,
        private companySvc: CompanyService,
        private messagesSvc: MessagesService
    ) {}

    public ngOnInit(): void {
        this.companySvc
            .getTeams()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe((teams) => {
                this.teams = teams;
            });

        this.route.queryParamMap
            .pipe(
                takeUntilDestroyed(this.destroyed),
                switchMap((q) => {
                    if (q.has('team')) {
                        this.team = Number.parseInt(q.get('team')!);
                    } else {
                        this.team = null;
                    }

                    return this.usersSvc.getUsers(this.team);
                })
            )
            .subscribe({
                next: (users) => {
                    this.users = users;
                },
                error: (e: HttpErrorResponse) => {
                    this.messagesSvc.isError('Unable to retrieve users');
                },
            });
    }
}
