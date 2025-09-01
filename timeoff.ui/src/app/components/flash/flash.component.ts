import { Component, OnInit, computed, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';

import { MessagesService } from '@services/messages/messages.service';
import { FlashModel } from './flash.model';

@Component({
    standalone: true,
    selector: 'flash-message',
    imports: [],
    providers: [],
    template: `@for (msg of errors(); track $index) {
        <div class="alert alert-danger" role="alert">{{ msg }}</div>
        } @for (msg of success(); track $index) {
        <div class="alert alert-success" role="alert">{{ msg }}</div>
        } `,
})
export class FlashComponent implements OnInit {
    readonly #msgSvc = inject(MessagesService);

    readonly #messages = toSignal(this.#msgSvc.getMessages(), { initialValue: new FlashModel() });

    protected readonly errors = computed(() => {
        if (this.#messages().isError) {
            return this.#messages().messages;
        } else {
            return [];
        }
    });

    protected readonly success = computed(() => {
        if (!this.#messages().isError) {
            return this.#messages().messages;
        } else {
            return [];
        }
    });

    public ngOnInit(): void {
        this.#msgSvc.clearStored();
    }
}