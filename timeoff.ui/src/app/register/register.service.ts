import { DestroyRef, Injectable } from '@angular/core';
import countries from '../services/countries.json'
import { RegisterOptions } from './register-options.model';
import { Observable, map, of, pipe } from 'rxjs';
import { RegisterModel } from './register.model';
import { HttpClient } from '@angular/common/http';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { getTimezones } from '../services/timezones';

@Injectable()
export class RegisterService {
  constructor(
    private readonly client: HttpClient,
    private destroyed: DestroyRef)
  {}

  public getOptions(): RegisterOptions{
    return {
      countries: countries,
      timezones: getTimezones(),
    };
  }

  public register(model: RegisterModel) : Observable<void> {
    return this.client.post<void>('/api/register',{
      ...model,
      confirmPassword: model.password
    }).pipe(
      takeUntilDestroyed(this.destroyed)
    );
  }
}
