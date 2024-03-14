import { Injectable } from '@angular/core';
import { FlashModel } from '../../components/flash/flash.model';

@Injectable({
  providedIn: 'root',
})
export class MessagesService {
  private messages: FlashModel[] = [];

  public isSuccess(value: string) {
    this.messages = [{
      isError: false,
      messages: [value],
    }];
  }

  public isError(value: string) {
    this.messages = [{
      isError: true,
      messages: [value],
    }];
  }

  public addMessage(model: FlashModel) {
    this.messages.push(model);
  }

  public getMessages() {
    if (this.messages.length == 0) {
      return new FlashModel();
    } else {
      return this.messages.pop()!;
    }
  }
}
