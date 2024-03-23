import { CommonModule } from '@angular/common';
import { Component, DestroyRef, Input, OnInit } from '@angular/core';
import { FlashModel } from './flash.model';
import { MessagesService } from '../../services/messages/messages.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
    standalone: true,
    templateUrl: 'flash.component.html',
    selector: 'flash-message',
    imports: [CommonModule],
    providers: [],
})
export class FlashComponent implements OnInit {
    private messages: FlashModel = new FlashModel();

    protected get errors(): string[] {
        if (this.messages.isError) {
            return this.messages.messages;
        } else {
            return [];
        }
    }

    protected get success(): string[] {
        if (!this.messages.isError) {
            return this.messages.messages;
        } else {
            return [];
        }
    }
    constructor(private readonly msgSvc: MessagesService, private destroyed: DestroyRef) {}

    public ngOnInit(): void {
        this.msgSvc
            .getMessages()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe((m) => {
                this.messages = m;
            });

        this.msgSvc.clearStored();
    }
}
