import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { form, required } from '@angular/forms/signals';

import { injectApi } from '@app-types/apiResource';
import { dateString } from '@app-types/dateString';

import { CalendarDayModel } from './calendar-day.model';

interface AddModel {
  id: number | null;
  name: string;
  date: dateString | null;
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
      publicHolidays: [form],
    }),
  );

  public delete = injectApi((id: number) => this.#httpClient.delete(`/api/public-holidays/${id}`));

  public getHolidays(p: () => number) {
    return rxResource({
      params: p,
      stream: (params) => {
        return this.#httpClient.get<CalendarDayModel[]>(`/api/public-holidays/${params.params}`);
      },
    });
  }

  public createAddForm() {
    const model = signal<AddModel>({
      id: null,
      name: '',
      date: null,
    });

    return form(model, (schema) => {
      required(schema.name);
      required(schema.date);
    });
  }
}
