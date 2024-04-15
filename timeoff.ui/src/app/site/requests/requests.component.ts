import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

import { FlashComponent } from "@components/flash/flash.component";
import { RequestsListComponent } from "@components/requests-list/requests-list.component";

import { LeaveRequestModel } from '@models/leave-request.model';
import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';

@Component({
    selector: 'app-requests',
    standalone: true,
    templateUrl: './requests.component.html',
    styleUrl: './requests.component.scss',
    imports: [FlashComponent, RequestsListComponent, CommonModule]
})
export class RequestsComponent {
    public get name(){
        return this.currentUser.userName;
    }

    public pending: LeaveRequestModel[] = [];

    constructor(private readonly currentUser: LoggedInUserService) {}
}
