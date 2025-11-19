import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { NonNullableFormBuilder, Validators } from '@angular/forms';

import { injectApi } from '@app-types/apiResource';
import { dateString } from '@app-types/dateString';

import { UserListModel } from './user-list.model';

type UserForm = ReturnType<UsersApi['createUserForm']>['value'];
type AdjustmentForm = ReturnType<UsersApi['createAdjustmentForm']>['value'];

@Injectable({
  providedIn: 'root',
})
export class UsersApi {
  readonly #httpClient = inject(HttpClient);

  readonly #fb = inject(NonNullableFormBuilder);

  public readonly updateSchedule = injectApi((form: UserForm['schedule']|null, id: number)=>this.#httpClient.put<UserForm['schedule']>(`/api/users/${id}/schedule`, form));

  public readonly createOrUpdate = injectApi((form: UserForm, id: number) => {
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
    this.#httpClient.delete<void>(`/api/users/${id}`)
  );

  public readonly resetPassword = injectApi((id: number) =>
    this.#httpClient.post<void>(`/api/users/${id}/reset-password`, {})
  );

  public readonly updateAdjustments = injectApi((form: AdjustmentForm, id: number) =>
    this.#httpClient.put<void>(`/api/users/${id}/adjustments`, form)
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

  public getAbsences(p:()=> number){
    return rxResource({
      params: p,
      stream:(params)=>this.#httpClient.get<object>(`/api/users/${params.params}/absences`)
    })
  }

  public createUserForm() {
    const form = this.#fb.group(
      {
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        team: [null as number | null],
        isAdmin: [false],
        autoApprove: [false],
        startDate: [null as dateString | null, Validators.required],
        endDate: [null as dateString | null],
        isActive: [true],
        schedule: this.#fb.group({
          monday: [false],
          tuesday: [false],
          wednesday: [false],
          thursday: [false],
          friday: [false],
          saturday: [false],
          sunday: [false],
        }),
        scheduleOverride: [false],
      },
      {
        validators: [],
      }
    );

    return form;
  }

  public createAdjustmentForm() {
    const form = this.#fb.group({
      carryOver: [0],
      adjustment: [0],
    });

    return form;
  }
}
