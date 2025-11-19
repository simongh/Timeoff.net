import { computed, DestroyRef, inject, ResourceStatus, signal, Signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Observable, Observer } from 'rxjs';

export interface ApiResource<TApi extends apiFn, TValue = ExtractObservable<ReturnType<TApi>>> {
  readonly value: Signal<TValue | undefined>;

  readonly status: Signal<ResourceStatus>;

  readonly error: Signal<unknown | undefined>;

  readonly isLoading: Signal<boolean>;

  load(f: { subscriber: Partial<Observer<TValue>>; payload: Parameters<TApi> }): void;

  hasValue(): boolean;
}

export class ApiResourceRef<TApi extends apiFn, TValue = ExtractObservable<ReturnType<TApi>>>
  implements ApiResource<TApi, TValue>
{
  readonly #destroyed = inject(DestroyRef);

  readonly #value = signal<TValue | undefined>(undefined);

  readonly #error = signal(undefined);

  readonly #status = signal<ResourceStatus>('idle');

  public readonly value = this.#value.asReadonly();

  public readonly status = this.#status.asReadonly();

  public readonly error = this.#error.asReadonly();

  public readonly isLoading = computed(
    () => this.#status() == 'loading' || this.#status() == 'reloading'
  );

  constructor(private request: TApi) {}

  public load(f: { subscriber: Partial<Observer<TValue>>; payload: Parameters<TApi> }): void {
    this.#error.set(undefined);
    this.#status.set('loading');

    this.request(...f.payload)
      .pipe(takeUntilDestroyed(this.#destroyed))
      .subscribe({
        next: (value) => {
          this.#status.set('resolved');
          this.#value.set(value);

          if (f.subscriber.next) {
            f.subscriber.next(value);
          }
        },
        error: (err) => {
          this.#status.set('error');
          this.#error.set(err);

          if (f.subscriber.error) {
            f.subscriber.error(err);
          }
        },
      });
  }

  public hasValue() {
    return (
      this.status() == 'local' ||
      this.status() == 'resolved' ||
      this.status() == 'reloading'
    );
  }
}

type ExtractObservable<T> = T extends Observable<infer I> ? I : never;

type apiFn = (...args: any[]) => Observable<any>;

export function injectApi<TMethod extends apiFn, TValue = ExtractObservable<ReturnType<TMethod>>>(
  request: TMethod
): ApiResource<TMethod, TValue> {
  return new ApiResourceRef<TMethod, TValue>(request);
}
