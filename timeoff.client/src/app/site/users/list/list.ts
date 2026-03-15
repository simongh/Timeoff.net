import { Component, inject, numberAttribute } from '@angular/core';
import { RouterLink } from '@angular/router';
import { derivedAsync } from 'ngxtension/derived-async';
import { injectQueryParams } from 'ngxtension/inject-query-params';

import { Card } from '@components/cards';
import { PageHeader } from '@components/page-header/page-header';

import { SiteApi } from '@api/site/site-api';
import { AuthService } from '@app-types/auth/auth.service';
import { YesPipe } from '@app-types/yes.pipe';

import { UsersApi } from '../users-api/users-api';

@Component({
  selector: 'ton-list',
  imports: [RouterLink, YesPipe, PageHeader, Card],
  templateUrl: './list.html',
  styleUrl: './list.scss',
})
export class List {
  readonly #usersSvc = inject(UsersApi);

  readonly #siteSvc = inject(SiteApi);

  protected readonly company = inject(AuthService).companyName;

  protected readonly team = injectQueryParams((p) =>
    p['team'] ? numberAttribute(p['team']) : null,
  );

  protected readonly teams = derivedAsync(() => this.#siteSvc.getTeams(), { initialValue: [] });

  protected readonly users = derivedAsync(
    () => this.#usersSvc.getUsers(() => this.team()).value(),
    {
      initialValue: [],
    },
  );
}
