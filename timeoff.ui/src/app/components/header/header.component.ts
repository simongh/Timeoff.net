import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';

import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';

@Component({
    standalone: true,
    templateUrl: 'header.component.html',
    selector: 'site-header',
    imports: [CommonModule, RouterLink],
})
export class HeaderComponent {
    private currentUser = inject(LoggedInUserService);

    protected readonly showTeamView = this.currentUser.showTeamView;

    protected readonly isAdmin = this.currentUser.isAdmin;

    protected readonly isSignedIn = this.currentUser.isUserLoggedIn;
}
