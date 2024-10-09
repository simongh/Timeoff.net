import { Component, inject, numberAttribute } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { injectQueryParams } from 'ngxtension/inject-query-params';
import { derivedAsync } from 'ngxtension/derived-async';
import { TippyDirective } from '@ngneat/helipopper';

import { FlashComponent } from '@components/flash/flash.component';
import { YesPipe } from '@components/yes.pipe';

import { CompanyService } from '@services/company/company.service';
import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';

import { UsersService } from '../users.service';

@Component({
    selector: 'user-list',
    standalone: true,
    templateUrl: './user-list.component.html',
    styleUrl: './user-list.component.scss',
    providers: [UsersService, CompanyService],
    imports: [FlashComponent, RouterLink, CommonModule, YesPipe, TippyDirective],
})
export class UserListComponent {
    protected readonly name = inject(LoggedInUserService).companyName;

    protected readonly teams = derivedAsync(() => this.companySvc.getTeams(), { initialValue: [] });

    protected readonly team = injectQueryParams((p) => (p['team'] ? numberAttribute(p['team']) : null));

    protected readonly users = derivedAsync(() => this.usersSvc.getUsers(this.team()), { initialValue: [] });

    constructor(private readonly usersSvc: UsersService, private readonly companySvc: CompanyService) {}
}
