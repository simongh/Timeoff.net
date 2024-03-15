import { FormControl } from "@angular/forms";

export interface DayModel{
    name: string;
    displayName: string;
    active: boolean;
    ctrl: FormControl<boolean>;
}