import { HttpClient } from '@angular/common/http';
import { inject, Injectable, Signal, signal, WritableSignal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { applyEach, form, required, SchemaPathTree } from '@angular/forms/signals';
import { it } from 'date-fns/locale';
import { map } from 'rxjs';

import { injectApi } from '@app-types/apiResource';
import { dateString } from '@app-types/dateString';

import { CalendarDayModel } from '../../../../types/calendar-day.model';

export interface AddModel {
  id: number | null;
  name: string;
  date: dateString;
}

@Injectable({
  providedIn: 'root',
})
export class PublicHolidaysApi {
  readonly #httpClient = inject(HttpClient);

  public addNew = injectApi((form: AddModel) =>
    this.#httpClient.post<void>('/api/public-holidays', {
      publicHolidays: [form],
    }),
  );

  public update = injectApi((form: AddModel[]) =>
    this.#httpClient.put<void>('/api/public-holidays', {
      publicHolidays: form,
    }),
  );

  public delete = injectApi((id: number) => this.#httpClient.delete(`/api/public-holidays/${id}`));

  public getHolidays(p: () => number) {
    return rxResource({
      params: p,
      stream: (params) => {
        return this.#httpClient
          .get<CalendarDayModel[]>(`/api/public-holidays/${params.params}`);
      },
    });
  }


  public createAddForm() {
    const model = signal<AddModel>({
      id: null,
      name: '',
      date: '',
    });

    return form(model, this.AddModelValidator);
  }

  public createEditForm(holidays: WritableSignal<AddModel[]>) {
    return form(holidays, (schema) => {
      applyEach(schema, this.AddModelValidator);
    });
  }

  private AddModelValidator(item: SchemaPathTree<AddModel>)
  {
    required(item.date);
    required(item.name);
  }
}
