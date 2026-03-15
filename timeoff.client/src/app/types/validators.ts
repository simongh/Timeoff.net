import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { SchemaPath, validate } from '@angular/forms/signals';

export function compareValidator(field1: string, field2: string): ValidatorFn {
  return (form: AbstractControl): ValidationErrors | null => {
    return form.get(field1)?.value === form.get(field2)?.value
      ? null
      : {
          notMatched: true,
        };
  };
}

export function compare(path: SchemaPath<string>, compareTo: SchemaPath<string>) {
  validate(path, ({ value, valueOf }) => {
    const confirmPassword = value();
    const password = valueOf(compareTo);

    if (confirmPassword !== password) {
      return {
        kind: 'passwordMismatch',
        message: 'Passwords do not match',
      };
    }

    return null;
  });
}
