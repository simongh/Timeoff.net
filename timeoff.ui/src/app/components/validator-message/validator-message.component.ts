import { NgIf } from '@angular/common';
import { Component, Host, Input, Optional, SkipSelf } from '@angular/core';
import { ControlContainer, FormControl } from '@angular/forms';

@Component({
    selector: 'validator-message',
    standalone: true,
    imports: [NgIf],
    templateUrl: './validator-message.component.html',
    styleUrl: './validator-message.component.scss',
})
export class ValidatorMessageComponent {
    @Input()
    public control!: FormControl;

    @Input()
    public controlName!: string;

    @Input()
    public validatorName: string | null = null;

    private parent: ControlContainer;

    constructor(@Optional() @Host() @SkipSelf() parent: ControlContainer) {
        this.parent = parent;
    }

    protected get hasError(): boolean {
        const control = !!this.control ? this.control : this.parent.control?.get(this.controlName);

        if (!control) {
            throw new Error(`Control was not found for validator ${this.validatorName}`);
        }

        if (control.touched) {
            if (this.validatorName) {
                return control.hasError(this.validatorName) || (control.parent?.hasError(this.validatorName) ?? false);
            } else {
                return control.errors != null;
            }
        } else {
            return false;
        }
    }
}
