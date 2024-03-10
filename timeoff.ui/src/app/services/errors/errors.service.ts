import { Injectable } from "@angular/core";

@Injectable()
export class ErrorsService {
    public messages: string[] = [];

    public errors: string[] = [];

    public addErrors(errors: string[])
    {
        this.errors = errors;
    }

    public addError(error: string)
    {
        this.errors.push(error);
    }

    public addMessage(message: string)
    {
        this.messages.push(message);
    }

    public reset() {
        this.errors = [];
        this.messages = [];
    }
}