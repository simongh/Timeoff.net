import { FormControl } from "@angular/forms";

export interface DayModel {
    name: string;
    displayName: string;
    control: FormControl<boolean>;
}
