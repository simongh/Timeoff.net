import { CommonModule } from '@angular/common';
import { Component, DestroyRef, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { combineLatest } from 'rxjs';
import { Country } from '../../../services/company/country.model';
import { dateFormats } from '../../../components/date-formats';
import { TimeZoneModel } from '../../../services/company/time-zone.model';
import { ColourPickerComponent } from './colour-picker/colour-picker.component';
import { LeaveTypeModalComponent } from './leave-type-modal/leave-type-modal.component';
import { FlashComponent } from '../../../components/flash/flash.component';
import { ScheduleComponent } from '../../../components/schedule/schedule.component';
import { CompanyService } from '../../../services/company/company.service';
import { MessagesService } from '../../../services/messages/messages.service';

@Component({
    selector: 'app-home',
    standalone: true,
    providers: [CompanyService],
    templateUrl: './home.component.html',
    styleUrl: './home.component.sass',
    imports: [
        CommonModule,
        ReactiveFormsModule,
        ColourPickerComponent,
        LeaveTypeModalComponent,
        RouterLink,
        FlashComponent,
        ScheduleComponent,
    ],
})
export class HomeComponent implements OnInit {
    public countries!: Country[];

    public dateFormats: string[] = dateFormats;

    public timeZones!: TimeZoneModel[];

    public carryOverDays: number[] = [];

    public get currentYear() {
        return new Date().getFullYear();
    }

    public get lastYear() {
        return this.currentYear - 1;
    }

    public companyName: string = '';

    public get settingsForm() {
        return this.companySvc.settingsForm;
    }

    public get leaveTypes() {
        return this.companySvc.leaveTypes;
    }

    public get days() {
        return this.companySvc.settingsForm.controls.schedule.controls;
    }

    public submitting = false;

    constructor(
        private destroyed: DestroyRef,
        private readonly companySvc: CompanyService,
        private readonly msgsSvc: MessagesService
    ) {
        for (let i = 0; i < 22; i++) {
            this.carryOverDays.push(i);
        }
        this.carryOverDays.push(1000);
    }

    public ngOnInit(): void {
        combineLatest([this.companySvc.timeZones(), this.companySvc.countries(), this.companySvc.getSettings()])
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe(([timeZones, countries, data]) => {
                this.timeZones = timeZones;
                this.countries = countries;

                this.settingsForm.setValue({
                    name: data.name,
                    dateFormat: data.dateFormat,
                    timeZone: data.timeZone,
                    hideTeamView: data.hideTeamView,
                    showHoliday: data.showHoliday,
                    country: data.country,
                    carryOver: data.carryOver,
                    schedule: [],
                });

                this.companySvc.fillSchedule(data.schedule);
                this.companySvc.fillLeaveTypes(data.leaveTypes);
            });
    }

    public save() {
        this.settingsForm.markAllAsTouched();

        if (this.settingsForm.invalid) {
            return;
        }

        this.submitting = true;

        this.companySvc
            .saveSettings()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.submitting = false;
                    this.msgsSvc.isSuccess('Company details updated');
                },
            });
    }
}
