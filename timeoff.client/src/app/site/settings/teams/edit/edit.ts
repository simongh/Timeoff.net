import { Component, inject, linkedSignal, numberAttribute } from '@angular/core';
import { injectParams } from 'ngxtension/inject-params';
import { RouterLink } from '@angular/router';
import { FormField } from '@angular/forms/signals';

import { PageHeader } from '@components/page-header/page-header';
import { Messages } from '@components/messages/messages';
import { AddModel, TeamsApi } from '../teams-api';

@Component({
  imports: [PageHeader, Messages, RouterLink, FormField],
  selector: 'ton-edit',
  styleUrl: './edit.scss',
  templateUrl: './edit.html',
})
export class Edit {
  readonly #teamsApi = inject(TeamsApi);

  protected readonly id = injectParams((p) => numberAttribute(p['id']));

  protected readonly team = this.#teamsApi.get(() => this.id());

  protected readonly model = linkedSignal(() => this.team.value());

  protected readonly form = this.#teamsApi.createEditForm(this.model);
}
