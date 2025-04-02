import { inject, Injectable, OnDestroy } from '@angular/core';
import { HubConnectionBuilder, HubConnectionState, LogLevel } from '@microsoft/signalr';

import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';
import { derivedAsync } from 'ngxtension/derived-async';
import { filter, map, startWith, Subject, tap } from 'rxjs';

@Injectable()
export class NotificationsService implements OnDestroy {
    readonly #userSvc = inject(LoggedInUserService);

    #hub = this.create();

    public readonly count = derivedAsync(() => {
        return this.#hub.state$.pipe(
            tap(() => {
                this.#hub.subscribeToMessage('AwaitingApproval')
                this.#hub.connect('/subs/requests', this.#userSvc.token()!, {})
            }),
            filter((value) => value.event == 'message-received' && value.messageType === 'AwaitingApproval'),
            map((value) => {
                if (value.event == 'message-received') {
                    return value.payload[0] as number;
                } else {
                    return 0;
                }
            }),
            startWith(0)
        );
    }, {
        initialValue: 0
    });

    ngOnDestroy(): void {
        this.#hub.disconnect();
    }

    private create() {
        let hub: signalR.HubConnection | undefined;

        let trackedTypes: string[] = [];
        const stream = new Subject<SignalRState>();

        stream.next({ event: 'disconnected' });

        const watchMessage = (messageType: string) => {
            if (!hub) {
                throw new Error('Cannot watch for messages before connection has been established');
            }

            hub.on(messageType, (...args: any[]) => {
                stream.next({
                    event: 'message-received',
                    messageType,
                    payload: args,
                });
            });
            stream.next({ event: 'listening-for-message', messageType, trackedMessageTypes: trackedTypes });
        };

        const invokeStart = async (
            hubUrl: string,
            authToken: string,
            headers: signalR.MessageHeaders,
            reconnect = false
        ): Promise<void> => {
            if (hub?.state == HubConnectionState.Connected) {
                stream.next({ event: 'already-connected' });
                return;
            }

            let builder = new HubConnectionBuilder()
                .withUrl(hubUrl, {
                    headers: headers,
                    accessTokenFactory: () => authToken,
                })
                .configureLogging(LogLevel.Information);

            if (reconnect) {
                builder = builder.withAutomaticReconnect();
            }

            hub = builder.build();

            hub.onclose(() => {
                stream.next({ event: 'disconnected' });
            });

            hub.onreconnecting(() => {
                stream.next({ event: 'reconnecting' });
            });

            hub.onreconnected(() => {
                stream.next({ event: 'reconnected' });
            });

            trackedTypes.forEach((messageType) => {
                watchMessage(messageType);
            });

            stream.next({ event: 'connecting' });
            hub.start()
                .then(() => {
                    stream.next({ event: 'connected' });
                })
                .catch((error) => {
                    stream.error(error);
                });
        };

        return {
            state$: stream,
            connect: invokeStart,
            subscribeToMessage: (messageType: string) => {
                if (trackedTypes.includes(messageType)) {
                    return;
                }

                trackedTypes.push(messageType);
                stream.next({ event: 'message-added-to-watch-list', messageType, trackedMessageTypes: trackedTypes });

                if (hub) {
                    console.warn(
                        `SignalR: Subscribing to message ${messageType} after the connection has been established. Messages might have been lost`
                    );
                    watchMessage(messageType);
                }
            },
            unsubscriberFromMessage: (messageType: string) => {
                if (!trackedTypes.includes(messageType)) {
                    return;
                }

                trackedTypes = trackedTypes.filter((m) => m !== messageType);
                stream.next({
                    event: 'message-removed-from-watch-list',
                    messageType,
                    trackedMessageTypes: trackedTypes,
                });

                if (hub) {
                    hub.off(messageType);
                    stream.next({
                        event: 'stopped-listening-for-message',
                        messageType,
                        trackedMessageTypes: trackedTypes,
                    });
                }
            },
            disconnect: (): void => {
                hub?.stop();
            },
        };
    }
}

export type SignalRState =
    | {
          event: 'connecting';
      }
    | {
          event: 'connected';
      }
    | {
          event: 'already-connected';
      }
    | {
          event: 'message-added-to-watch-list';
          messageType: string;
          trackedMessageTypes: string[];
      }
    | {
          event: 'message-removed-from-watch-list';
          messageType: string;
          trackedMessageTypes: string[];
      }
    | {
          event: 'listening-for-message';
          messageType: string;
          trackedMessageTypes: string[];
      }
    | {
          event: 'stopped-listening-for-message';
          messageType: string;
          trackedMessageTypes: string[];
      }
    | {
          event: 'message-received';
          messageType: string;
          payload: any[];
      }
    | {
          event: 'reconnecting';
      }
    | {
          event: 'reconnected';
      }
    | {
          event: 'disconnected';
      };
