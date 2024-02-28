import { Component, OnInit } from '@angular/core';
import { Country } from '../../../services/country.model';
import countries from '../../../services/countries.json'
import { CommonModule } from '@angular/common';
import { getDateFormats } from './date-formats';
import { TimezoneModel } from '../../../services/timezone.model';
import { getTimezones } from '../../../services/timezones';
import { SettingsService } from './settings-service';
import { ReactiveFormsModule } from '@angular/forms';
import { ColourPickerComponent } from "./colour-picker/colour-picker.component";
import { LeaveTypeModalComponent } from "./leave-type-modal/leave-type-modal.component";
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-home',
    standalone: true,
    providers: [SettingsService],
    templateUrl: './home.component.html',
    styleUrl: './home.component.sass',
    imports: [CommonModule, ReactiveFormsModule, ColourPickerComponent, LeaveTypeModalComponent, RouterLink]
})
export class HomeComponent implements OnInit {
  public countries!: Country[];
  public dateFormats!: string[];
  public timezones!: TimezoneModel[];
  public carryOverDays: number[] = [];
  public get currentYear() {
    return new Date().getFullYear();
  }
  public get lastYear() {
    return this.currentYear - 1;
  }

  public companyName: string = '';

  public get settingsForm() {
    return this.settingsService.form;
  }

  public get leaveTypes() {
    return this.settingsService.leaveTypes;
  }

  constructor(private readonly settingsService: SettingsService)
  {  }

  public ngOnInit(): void {
    this.countries = countries;
    this.dateFormats = getDateFormats();
    this.timezones = getTimezones();

    for(let i = 0; i < 22; i++) {
      this.carryOverDays.push(i);
    }
    this.carryOverDays.push(1000);
  }

}
