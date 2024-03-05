import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PublicHolidayModel } from './public-holiday.model';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { yearValidator } from '../../components/validators';

export interface holidayFormControls {
  id: FormControl<number|null>;
  name: FormControl<string|null>;
  date: FormControl<Date|null>;
}

@Injectable()
export class PublicHolidaysService {
  public form = this.newForm({
    id: null,
    name: '',
    date:null,
  },0)

  public holidays = this.fb.array<FormGroup<holidayFormControls>>([]);

  constructor(
    private readonly client: HttpClient,
    private readonly fb: FormBuilder
  ) { }

  public get(year: number) {
    return this.client.get<PublicHolidayModel[]>(`/api/public-holidays/${year}`);
  }

  public newForm(data: PublicHolidayModel, year: number) {
    return this.fb.group({
      id: data.id,
      name: [data.name, Validators.required],
      date: [data.date, yearValidator(year)]
    });
  }

  public add() {
    return this.client.post<void>('/api/public-holidays',this.form.value);
  }

  public update() {
    return this.client.put<void>('/api/public-holidays',this.holidays.value);
  }

  public delete(id: number) {
    return this.client.delete(`/api/public-holidays/${id}`);
  }
}
