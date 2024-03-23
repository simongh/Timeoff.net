import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
    standalone: true,
    templateUrl: 'colour-picker.component.html',
    selector: 'colour-picker',
    imports: [CommonModule],
})
export class ColourPickerComponent {
    public get colour() {
        return this.control?.value;
    }

    @Input()
    public control!: FormControl<string | null>;

    public pick(colour: string) {
        this.control.setValue(colour);
    }
}
