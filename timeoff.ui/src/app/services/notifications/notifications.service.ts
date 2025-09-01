import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { HubConnection, HubConnectionBuilder, HubConnectionState, LogLevel } from '@microsoft/signalr';

import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';

@Injectable({
    providedIn: 'root'
})
export class NotificationsService {
    private hub: HubConnection | null = null;

    public readonly count = signal(0);

    constructor(private readonly currentUser: LoggedInUserService) {
        return;
        toObservable(this.currentUser.isUserLoggedIn).subscribe((value) => {
            if (value) {
                this.start();
            } else {
                this.stop();
            }
        });
    }

    private start() {
        if (!this.hub) {
            this.newHub();
        }

        if (this.hub!.state == HubConnectionState.Connected) {
            return;
        }
    }

    private stop() {
        if (!this.hub) {
            return;
        }

        this.hub.stop();
        this.hub = null;
    }

    private newHub() {
        this.hub = new HubConnectionBuilder()
            .withUrl('/hubs/requests', { accessTokenFactory: () => this.currentUser.token()! })
            .configureLogging(LogLevel.Debug)
            .build();

        this.hub.on('AwaitingApproval', (count: number) => {
            this.count.set(count);
        });

        this.hub.start();
    }
}
