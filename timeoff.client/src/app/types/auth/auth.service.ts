import { HttpClient, HttpContext } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { differenceInSeconds, parseISO } from 'date-fns';
import { catchError, of, switchMap, tap } from 'rxjs';

import { LoggedInUserModel } from '@api/auth/logged-in-user.model';

import { BYPASS_TOKEN } from './auth.interceptor';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  readonly #client = inject(HttpClient);

  readonly #user = signal({} as LoggedInUserModel);

  public readonly companyName = computed(() => this.#user().companyName || '');

  public readonly userName = computed(() => this.#user().name || '');

  public readonly showTeamView = computed(() => !!this.#user().showTeamView);

  public readonly isAdmin = computed(() => !!this.#user().isAdmin);

  public readonly token = computed(() => this.#user().token ?? null);

  public readonly isUserLoggedIn = computed(() => !!this.token());

  public readonly dateFormat = computed(() => {
    const value = this.#user().dateFormat;
    return value || 'yyyy-MM-dd';
  });

  public needsExtending() {
    const value = this.#user().expires;

    if (!value) {
      return true;
    }

    return differenceInSeconds(parseISO(value), Date.now()) < 60;
  }

  public load(user: LoggedInUserModel) {
    this.#user.set(user);
  }

  public clear() {
    this.#user.set({} as LoggedInUserModel);
  }

  public extend() {
    return this.#client
      .get<LoggedInUserModel>('/api/auth/token', {
        context: new HttpContext().set(BYPASS_TOKEN, true),
      })
      .pipe(
        catchError((e) => of(null)),
        tap((u) => this.#user.set(u ?? ({} as LoggedInUserModel)))
      );
  }

  public logout() {
    return this.#client
      .post(
        '/api/auth/logout',
        {},
        {
          context: new HttpContext().set(BYPASS_TOKEN, true),
        }
      )
      .pipe(
        switchMap(() => {
          this.clear();

          return of(null);
        })
      );
  }
}
