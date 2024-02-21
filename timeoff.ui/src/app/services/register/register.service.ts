import { Injectable } from '@angular/core';
import countries from '../countries.json'
import { RegisterOptions } from './register-options.model';
import { Observable, of } from 'rxjs';
import { RegisterModel } from './register.model';

@Injectable()
export class RegisterService {
  public getOptions(): RegisterOptions{
    return {
      countries: countries,
      timezones: [{
        name: 'GMT Standard Time',
        description: '(UTC+00:00) Dublin, Edinburgh, Lisbon, London'
      }],
    };
  }

  public register(model: RegisterModel) : Observable<boolean> {
    return of(true);
  }
}
