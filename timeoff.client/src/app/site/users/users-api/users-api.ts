import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

import { UserListModel } from './user-list.model';

@Injectable({
  providedIn: 'root',
})
export class UsersApi {
  readonly #httpClient = inject(HttpClient);

  public getUsers(team: number | null) {
    const options = team
      ? {
          params: new HttpParams().set('team', team),
        }
      : {};

    return this.#httpClient.get<UserListModel[]>('/api/users', options);
  }
}
