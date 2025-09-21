import { Component, computed, inject, input } from '@angular/core';
import { ControlContainer, FormControl } from '@angular/forms';

@Component({
  selector: 'ton-validator-message',
  imports: [],
  template: `@if (hasError) {
    <small class="text-danger"><ng-content></ng-content></small>
    } `,
  styleUrl: './validator-message.scss',
})
export class ValidatorMessage {
  readonly #parent = inject(ControlContainer, { optional: true, skipSelf: true, host: true });

  public readonly control = input<FormControl | null>(null);

  public readonly controlName = input<string | null>(null);

  public readonly validatorName = input<string | null>(null);

  protected get hasError() {
    const control = this.control() || this.#parent?.control?.get(this.controlName()!);

    if (!control) {
      throw new Error(`Control was not found for validator ${this.validatorName}`);
    }

    if (control.touched) {
      if (this.validatorName()) {
        return (
          control.hasError(this.validatorName()!) ||
          (control.parent?.hasError(this.validatorName()!) ?? false)
        );
      } else {
        return control.errors != null;
      }
    } else {
      return false;
    }
  };
}
