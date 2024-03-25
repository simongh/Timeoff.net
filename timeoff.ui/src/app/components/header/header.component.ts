import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';
import { RouterLink } from '@angular/router';

@Component({
    standalone: true,
    templateUrl: 'header.component.html',
    selector: 'site-header',
    imports: [CommonModule, RouterLink],
})
export class HeaderComponent {
    public showTeamView = true;

    public isAdmin = true;

    public get isSignedIn() {
        return this.authSvc.isUserLoggedIn;
    }

    constructor(private authSvc: AuthService) {}
}
