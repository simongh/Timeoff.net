import { CommonModule } from "@angular/common";
import { Component, Input } from "@angular/core";
import { ErrorsService } from "../../services/errors/errors.service";
import { FlashModel } from "./flash.model";

@Component({
    standalone: true,
    templateUrl: 'flash.component.html',
    selector: 'flash-message',
    imports: [CommonModule],
    providers: [ErrorsService]
})

export class FlashComponent {
    @Input()
    public messages: FlashModel = new FlashModel();

    protected get errors() : string[] {
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
    constructor() {}
}