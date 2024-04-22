import { CommonModule } from '@angular/common';
import { Component, DestroyRef, OnInit, computed, effect, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

import { MessagesService } from '@services/messages/messages.service';
import { FlashModel } from './flash.model';

@Component({
    standalone: true,
    templateUrl: 'flash.component.html',
    selector: 'flash-message',
    imports: [CommonModule],
    providers: [],
})
export class FlashComponent implements OnInit {
    private readonly messages = signal(new FlashModel());

    protected readonly errors = computed(() => {
        if (this.messages().isError) {
            return this.messages().messages;
        } else {
            return [];
        }
    });

    protected readonly success = computed(() => {
        if (!this.messages().isError) {
            return this.messages().messages;
        } else {
            return [];
        }
    });

    constructor(private readonly msgSvc: MessagesService, private destroyed: DestroyRef) {}

    public ngOnInit(): void {
        this.msgSvc
            .getMessages()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe((m) => {
                this.messages.set(m);
            });

        this.msgSvc.clearStored();
    }
}
