import { Component, computed, inject, input, model, ModelSignal } from '@angular/core';
import { ControlContainer, FormControl } from '@angular/forms';
import { FieldState, FieldTree, FormValueControl } from '@angular/forms/signals';

@Component({
  selector: 'ton-validator-message',
  imports: [],
  template: `@if (hasError()) {
    <small class="text-danger"><ng-content></ng-content></small>
  } `,
  styleUrl: './validator-message.scss',
})
export class ValidatorMessage<T> {
  public readonly field = input.required<FieldTree<T>>();

  public readonly validatorName = input<string | null>(null);

  protected readonly hasError = computed(() => {
    const errors = this.field()().errors();

    if (this.field()().touched()) {
      if (this.validatorName()) {
        return errors.some((e) => e.kind == this.validatorName());
      } else {
        return this.field()().invalid();
      }
    } else {
      return false;
    }
  });
}
