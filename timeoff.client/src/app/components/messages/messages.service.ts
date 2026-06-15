import { computed, Injectable, signal, untracked } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class MessagesService {
  readonly #store = signal<{
    type: 'success' | 'danger' | '';
    text: string;
  } | null>(null);

  public readonly message = computed(() => {
    const msg = this.#store();
    untracked(() => {
      this.#store.set(null);
    });
    return msg;
  });

  public clear() {
    this.#store.set({
      type: '',
      text: '',
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
