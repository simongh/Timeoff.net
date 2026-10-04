import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faChevronRight } from '@fortawesome/free-solid-svg-icons';

import { Messages } from '@components/messages/messages';
import { PageHeader } from '@components/page-header/page-header';
import { YesPipe } from '@app-types/yes.pipe';

import { TeamsApi } from '../teams-api';

@Component({
  imports: [PageHeader, RouterLink, YesPipe, FaIconComponent, Messages],
  selector: 'ton-list',
  styleUrl: './list.scss',
  templateUrl: './list.html',
})
export class List {
  readonly #teamsApi = inject(TeamsApi);

  protected readonly teams = this.#teamsApi.getAll();

  protected readonly faChevronRight = faChevronRight;
}

