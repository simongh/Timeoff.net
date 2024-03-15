import { Injectable } from '@angular/core';
import { FlashModel } from '../../components/flash/flash.model';
import { Subject } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class MessagesService {
    private stored = new FlashModel();

    private subject = new Subject<FlashModel>();

    public isSuccess(value: string) {
        this.addMessage({
            isError: false,
            messages: [value],
        });
    }

    public isError(value: string) {
        this.addMessage({
            isError: true,
            messages: [value],
        });
    }

    public hasErrors(values: string[]) {
        this.addMessage({
            isError: true,
            messages: values,
        });
    }

    public addMessage(model: FlashModel) {
        this.subject.next(model);
        this.stored = model;
    }

    public clearStored() {
        this.addMessage(this.stored);
        this.stored = new FlashModel();
    }

    public getMessages() {
        return this.subject.asObservable();
    }
}
