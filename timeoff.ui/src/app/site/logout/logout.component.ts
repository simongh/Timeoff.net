import { Component } from "@angular/core";
import { AuthService } from "../../services/auth/auth.service";
import { Router } from "@angular/router";

@Component({
    standalone: true,
    templateUrl: 'logout.component.html',
})
export class LogoutComponent {
    constructor(
        private authService: AuthService,
        private router: Router)
    {}

    public logout() {
        this.authService.logout();

        this.router.navigate(['']);
    }
}