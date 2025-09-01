import { HttpClient } from '@angular/common/http';
import { Injectable, computed, inject, signal } from '@angular/core';
import { Subject, catchError, of, tap } from 'rxjs';

import { LoggedInUserModel } from '@models/logged-in-user.model';
import { differenceInSeconds, getMinutes } from 'date-fns';

@Injectable({
    providedIn: 'root',
})
export class LoggedInUserService {
    readonly #client = inject(HttpClient);

    readonly #user = signal({} as LoggedInUserModel);

    public readonly companyName = computed(() => this.#user().companyName || '');

    public readonly userName = computed(() => this.#user().name || '');

    public readonly showTeamView = computed(() => !!this.#user().showTeamView);

    public readonly isAdmin = computed(() => !!this.#user().isAdmin);

    public readonly token = computed(() => this.#user().token ?? null);

    public readonly needsExtending = computed(() => {
        const value = this.#user().expires;

        if (!value) {
            return true;
        }

        return differenceInSeconds(new Date(value), new Date()) < 60;
    });

    public readonly dateFormat = computed(() => {
        const value = this.#user().dateFormat;
        return value || 'yyyy-MM-dd';
    });

    public readonly isUserLoggedIn = computed(() => !!this.token());

    public readonly refresh$ = new Subject();

    public load(user: LoggedInUserModel | null) {
        if (!user) {
            this.clear();
        } else {
            this.#user.set(user);
        }
    }

    public clear() {
        this.#user.set({} as LoggedInUserModel);
    }

    public extend() {
        return this.#client.get<LoggedInUserModel>('/api/account/token').pipe(
            catchError(() => {
                return of(null);
            }),
            tap((u) => this.load(u))
        );
    }

    public refresh() {
        this.refresh$.next([]);
    }
}