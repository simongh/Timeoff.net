import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export function compareValidator(field1: string, field2: string): ValidatorFn {
    return (form: AbstractControl) : ValidationErrors | null => {
      return form.get(field1)?.value === form.get(field2)?.value ? null : {
        notMatched : true
      };
    };
  }

export function listValidator(values: string[]): ValidatorFn {
    return (control: AbstractControl) : ValidationErrors | null => {
        return values.some((v)=> control.value === v) ? null : {
        invalidItem: true
        };
    };
}

export function yearValidator(year: number): ValidatorFn {
  return (control: AbstractControl) : ValidationErrors | null => {
    console.log(control.value?.substring(0,4) == year);
    return control.value?.substring(0,4) == year ? null : {
      invalidYear: true,
      year: year
    };
  };
}
