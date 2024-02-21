import { CommonModule } from "@angular/common";
import { Component, Input } from "@angular/core";

@Component({
    standalone: true,
    templateUrl: 'flash.component.html',
    selector: 'flash-message',
    imports: [CommonModule]
})

export class FlashComponent {
    @Input()
    public errors?: string[];
    @Input()
    public messages?: string[];
}