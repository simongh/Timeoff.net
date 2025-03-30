import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NgIf } from '@angular/common';

import { FooterComponent } from '@components/footer/footer.component';
import { HeaderComponent } from '@components/header/header.component';
import { AddNewModalComponent } from "@components/add-new-absence-modal/add-new-model.component";

import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrl: './app.component.scss',
    imports: [RouterOutlet, FooterComponent, HeaderComponent, AddNewModalComponent, NgIf]
})
export class AppComponent {
    protected readonly isLoggedIn = inject(LoggedInUserService).isUserLoggedIn
}
