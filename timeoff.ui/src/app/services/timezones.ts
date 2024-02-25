import { TimezoneModel } from "./timezone.model";

export function getTimezones(): TimezoneModel[] {
    return [{
        name: 'GMT Standard Time',
        description: '(UTC+00:00) Dublin, Edinburgh, Lisbon, London'
      }];
}