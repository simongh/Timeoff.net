import { CommonModule } from '@angular/common';
import { Component, computed, input } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
    standalone: true,
    templateUrl: 'colour-picker.component.html',
    selector: 'colour-picker',
    imports: [CommonModule],
})
export class ColourPickerComponent {
    protected get colour() {
        return this.control().value;
    }

    public readonly control = input.required<FormControl<string | null>>();

    public pick(colour: string) {
        this.control().setValue(colour);
    }
}
