import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';

import { CalendarWeekIcon, FileReportIcon, UsersIcon, BellIcon, SettingsIcon, UserCircleIcon } from '@components/svg';

import { AuthService } from '@app-types/auth/auth.service';


@Component({
  selector: 'ton-header',
  imports: [CalendarWeekIcon, FileReportIcon, UsersIcon, BellIcon, SettingsIcon, UserCircleIcon, NgbDropdownModule, RouterLink],
  templateUrl: './header.html',
  styleUrl: './header.scss'
})
export class Header {
  readonly #user = inject(AuthService);

  protected get isAdmin() {
    return this.#user.isAdmin;
  }

  protected get showTeamView() {
    return this.#user.showTeamView;
  }

  protected get username() {
    return this.#user.userName;
  }
}
