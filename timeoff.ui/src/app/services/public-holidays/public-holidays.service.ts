import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PublicHolidayModel } from './public-holiday.model';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { yearValidator } from '../../components/validators';
import { formatDate } from '@angular/common';

export interface holidayFormControls {
  id: FormControl<number|null>;
  name: FormControl<string|null>;
  date: FormControl<string|null>;
}

@Injectable()
export class PublicHolidaysService {
  public holidays = this.fb.array<FormGroup<holidayFormControls>>([]);

  public addForm : FormGroup<holidayFormControls> | null = null;

  constructor(
    private readonly client: HttpClient,
    private readonly fb: FormBuilder
  ) { 
    this.setAddForm(0);
  }

  public get(year: number) {
    return this.client.get<PublicHolidayModel[]>(`/api/public-holidays/${year}`);
  }

  public setAddForm(year: number) {
    this.addForm = this.newForm({
      id: null,
      name: '',
      date: null
    }, year);
  }

  public newForm(data: PublicHolidayModel, year: number) {
    return this.fb.group({
      id: data.id,
      name: [data.name, Validators.required],
      date: [data.date ? formatDate(data.date, 'yyyy-MM-dd','en') : null, yearValidator(year)]
    });
  }

  public add(model: PublicHolidayModel) {
    return this.client.post<void>('/api/public-holidays', {
      publicHolidays : [model]
    });
  }

  public update(model: PublicHolidayModel[]) {
    return this.client.put<void>('/api/public-holidays', {
      publicHolidays : model
    });
  }

  public delete(id: number) {
    return this.client.delete(`/api/public-holidays/${id}`);
  }
}
