import { Component, Input } from "@angular/core";

@Component({
    standalone: true,
    templateUrl: 'colour-picker.component.html',
    selector: 'colour-picker'
})
export class ColourPickerComponent{
    @Input()
    public colour!: string| null;

    public pick(colour: string) {
        this.colour = colour;

        return false;
    }
}