import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  public readonly toasts: Toast[] = [];

  public addSuccess(message: string) {
    this.toasts.push({
      message: message,
      class: 'bg-success text-light',
    });
  }

  public addFailure(message: string) {
    this.toasts.push({
      message: message,
      class: 'bg-danger text-light',
    });
  }

  public remove(toast: Toast) {
    const i = this.toasts.indexOf(toast);
    this.toasts.splice(i, 1);
  }
}

interface Toast {
  message: string;
  class: string;
}
