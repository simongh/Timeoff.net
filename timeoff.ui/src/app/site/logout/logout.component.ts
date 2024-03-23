import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';

@Component({
    standalone: true,
    template: '',
})
export class LogoutComponent implements OnInit {
    constructor(private authService: AuthService, private router: Router) {}

    public ngOnInit(): void {
        this.authService.logout().subscribe(() => this.router.navigateByUrl('/'));
    }
}
