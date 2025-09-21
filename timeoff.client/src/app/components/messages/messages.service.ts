import { computed, Injectable, signal, untracked } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class MessagesService {
  readonly #store = signal<{
    type: 'success' | 'danger';
    text: string;
  } | null>(null);

  public readonly message = computed(() => {
    const msg = this.#store();
    this.clear();
    return msg;
  });

  public clear() {
    untracked(() => {
      this.#store.set(null);
    });
  }

  public addSuccess(message: string) {
    this.#store.set({
      type: 'success',
      text: message,
    });
  }

  public addError(message: string) {
    this.#store.set({
      type: 'danger',
      text: message,
    });
  }
}
