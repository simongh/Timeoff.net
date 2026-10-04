import { HttpClient } from '@angular/common/http';
import { inject, Service, Signal, WritableSignal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { form, max, min, required } from '@angular/forms/signals';

import { injectApi } from '@app-types/apiResource';
import { TeamListModel } from './team-list.model';

export interface AddModel {
  name: string;
  manager: number;
  allowance: number;
  includePublicHolidays: boolean;
  isAccruedAllowance: boolean;
}

@Service()
export class TeamsApi {
  readonly #httpClient = inject(HttpClient);

  public readonly update = injectApi((id: number, form: AddModel) =>
    this.#httpClient.put<void>(`/api/teams/${id}`, form),
  );

  public getAll() {
    return rxResource({
      stream: () => {
        return this.#httpClient.get<TeamListModel[]>('/api/teams');
      },
    });
  }

  public get(p: () => number) {
    return rxResource({
      params: p,
      stream: (params) => {
        return this.#httpClient.get<AddModel>(`/api/teams/${params.params}`);
      },
      defaultValue: {
        name: ''
      } as AddModel
    });
  }

  public createEditForm(model: WritableSignal<AddModel>) {
    return form(model, (schema) => {
      required(schema.name);
      required(schema.manager);
      min(schema.allowance, 5);
      max(schema.allowance, 50);
    });
  }
}
