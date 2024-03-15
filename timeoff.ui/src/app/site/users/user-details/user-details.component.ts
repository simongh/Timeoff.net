import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { FlashComponent } from '../../../components/flash/flash.component';

@Component({
    selector: 'user-details',
    standalone: true,
    templateUrl: './user-details.component.html',
    styleUrl: './user-details.component.sass',
    imports: [CommonModule, RouterLink, FlashComponent, RouterLinkActive],
})
export class UserDetailsComponent {
    @Input()
    public name!: string;

    @Input()
    public isActive!: boolean;

    @Input()
    public id!: number;

    @Output()
    public deleting = new EventEmitter();

    public delete() {
        const doit = window.confirm(`Do you really want to delete the user ${this.name}?`);

        if (doit) {
            this.deleting.emit();
        }
    }
}
