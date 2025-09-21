import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function compareValidator(field1: string, field2: string): ValidatorFn {
  return (form: AbstractControl): ValidationErrors | null => {
    return form.get(field1)?.value === form.get(field2)?.value
      ? null
      : {
          notMatched: true,
        };
  };
}
