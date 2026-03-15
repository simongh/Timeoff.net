import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { NonNullableFormBuilder, Validators } from '@angular/forms';
import { email, form, required } from '@angular/forms/signals';
import { tr } from 'date-fns/locale';

import { injectApi } from '@app-types/apiResource';
import { dateString } from '@app-types/dateString';

import { UserListModel } from './user-list.model';

interface UserModel {
  firstName: string;
  lastName: string;
  email: string;
  team: number | null;
  isAdmin: boolean;
  autoApprove: boolean;
  startDate: dateString | null;
  endDate: dateString | null;
  isActive: boolean;
  schedule: {
    monday: boolean;
    tuesday: boolean;
    wednesday: boolean;
    thursday: boolean;
    friday: boolean;
    saturday: boolean;
    sunday: boolean;
  };
  scheduleOverride: boolean;
}
interface AdjustmentModel {
  carryOver: number;
  adjustment: number;
}

@Injectable({
  providedIn: 'root',
})
export class UsersApi {
  readonly #httpClient = inject(HttpClient);

  public readonly updateSchedule = injectApi((form: UserModel['schedule'] | null, id: number) =>
    this.#httpClient.put<UserModel['schedule']>(`/api/users/${id}/schedule`, form),
  );

  public readonly createOrUpdate = injectApi((form: UserModel, id: number) => {
    if (id === 0) {
      return this.#httpClient.post<void>('/api/users', form);
    } else {
      return this.#httpClient.put<void>(`/api/users/${id}`, {
        ...form,
        endDate: form.endDate == '' ? null : form.endDate,
      });
    }
  });

  public readonly delete = injectApi((id: number) =>
    this.#httpClient.delete<void>(`/api/users/${id}`),
  );

  public readonly resetPassword = injectApi((id: number) =>
    this.#httpClient.post<void>(`/api/users/${id}/reset-password`, {}),
  );

  public readonly updateAdjustments = injectApi((form: AdjustmentModel, id: number) =>
    this.#httpClient.put<void>(`/api/users/${id}/adjustments`, form),
  );

  public getUsers(p: () => number | null) {
    return rxResource({
      params: p,
      stream: (params) => {
        const options = params.params
          ? {
              params: new HttpParams().set('team', params.params),
            }
          : {};
        return this.#httpClient.get<UserListModel[]>('/api/users', options);
      },
    });
  }

  public getAbsences(p: () => number) {
    return rxResource({
      params: p,
      stream: (params) => this.#httpClient.get<object>(`/api/users/${params.params}/absences`),
    });
  }

  public createUserForm() {
    const model = signal<UserModel>({
      firstName: '',
      lastName:'',
      email: '',
      team: null,
      isAdmin: false,
      autoApprove: false,
      startDate: null,
      endDate: null,
      isActive: true,
      schedule: {
        monday: true,
        tuesday: true,
        wednesday: true,
        thursday: true,
        friday: true,
        saturday: false,
        sunday: false,
      },
      scheduleOverride: false
    });

    return form(model,(schema)=>{
      required(schema.firstName);
      required(schema.lastName);
      
      required(schema.email);
      email(schema.email);

      required(schema.startDate);
    });
  }

  public createAdjustmentForm() {
    const model = signal<AdjustmentModel>({
      carryOver: 0,
      adjustment: 0,
    });

    return form(model);
  }
}
