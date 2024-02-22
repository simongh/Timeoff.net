import { Component, OnInit } from "@angular/core";
import { AuthService } from "../../services/auth/auth.service";
import { Router } from "@angular/router";

@Component({
    standalone: true,
    template: ''
})
export class LogoutComponent implements OnInit {
    constructor(
        private authService: AuthService,
        private router: Router)
    {}

    public ngOnInit(): void {
        this.authService.logout().subscribe(()=> this.router.navigateByUrl('/'));
    }
}