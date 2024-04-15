import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';

@Component({
    standalone: true,
    templateUrl: 'header.component.html',
    selector: 'site-header',
    imports: [CommonModule, RouterLink],
})
export class HeaderComponent {
    public get showTeamView() {
        return this.currentUser.showTeamView;
    }

    public get isAdmin() {
        return this.currentUser.isAdmin;
    }

    public get isSignedIn() {
        return this.currentUser.isUserLoggedIn;
    }

    constructor(private currentUser: LoggedInUserService) {}
}
