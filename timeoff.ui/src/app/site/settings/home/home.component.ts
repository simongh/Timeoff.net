import { CommonModule } from '@angular/common';
import { Component, DestroyRef, OnInit, computed, signal } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { HttpErrorResponse } from '@angular/common/http';
import { combineLatest } from 'rxjs';

import { dateFormats } from '@models/date-formats';

import { FlashComponent } from '@components/flash/flash.component';
import { ScheduleComponent } from '@components/schedule/schedule.component';

import { CompanyService } from '@services/company/company.service';
import { MessagesService } from '@services/messages/messages.service';
import { TimeZoneModel } from '@services/company/time-zone.model';
import { Country } from '@services/company/country.model';

import { RemoveCompanyModalComponent } from './remove-company-modal/remove-company-modal.component';
import { ColourPickerComponent } from './colour-picker/colour-picker.component';
import { LeaveTypeModalComponent } from './leave-type-modal/leave-type-modal.component';

@Component({
    selector: 'app-home',
    standalone: true,
    providers: [CompanyService],
    templateUrl: './home.component.html',
    styleUrl: './home.component.scss',
    imports: [
        CommonModule,
        ReactiveFormsModule,
        ColourPickerComponent,
        LeaveTypeModalComponent,
        RouterLink,
        FlashComponent,
        ScheduleComponent,
        RemoveCompanyModalComponent,
    ],
})
export class HomeComponent implements OnInit {
    protected readonly countries = signal<Country[]>([]);

    protected readonly dateFormats = signal(dateFormats);

    protected readonly timeZones = signal<TimeZoneModel[]>([]);

    protected readonly carryOverDays = computed(() => {
        const days = Array<number>();
        for (let i = 0; i < 22; i++) {
            days.push(i);
        }
        days.push(1000);

        return days;
    });

    protected readonly currentYear = computed(() => new Date().getFullYear());

    protected readonly lastYear = computed(()=> this.currentYear() - 1);

    protected get companyName() {
        return this.companySvc.settingsForm.value.name;
    }

    public get settingsForm() {
        return this.companySvc.settingsForm;
    }

    public get leaveTypes() {
        return this.companySvc.leaveTypes;
    }

    public get leaveTypeForm() {
        return this.companySvc.leaveTypeForm;
    }

    public get firstType() {
        return this.companySvc.first;
    }

    public get days() {
        return this.companySvc.settingsForm.controls.schedule;
    }

    protected readonly submitting = signal(false);

    constructor(
        private destroyed: DestroyRef,
        private readonly router: Router,
        private readonly companySvc: CompanyService,
        private readonly msgsSvc: MessagesService
    ) {}

    public ngOnInit(): void {
        combineLatest([this.companySvc.timeZones(), this.companySvc.countries(), this.companySvc.getSettings()])
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe(([timeZones, countries, data]) => {
                this.timeZones.set(timeZones);
                this.countries.set(countries);

                this.settingsForm.setValue({
                    name: data.name,
                    dateFormat: data.dateFormat,
                    timeZone: data.timeZone,
                    hideTeamView: data.hideTeamView,
                    showHoliday: data.showHoliday,
                    country: data.country,
                    carryOver: data.carryOver,
                    schedule: data.schedule,
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

        this.submitting.set(true);

        this.companySvc
            .saveSettings()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.submitting.set(false);
                    this.msgsSvc.isSuccess('Company details updated');
                },
                error: (e: HttpErrorResponse) => {
                    this.submitting.set(false);
                    if (e.status == 400) {
                        this.msgsSvc.hasErrors(e.error.errors);
                    } else {
                        this.msgsSvc.isError('Unable to save changes');
                    }
                },
            });
    }

    public saveSchedule() {
        const form = this.settingsForm.controls.schedule;

        form.markAllAsTouched();
        if (form.invalid) {
            return;
        }

        this.submitting.set(true);

        this.companySvc
            .saveSchedule()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.msgsSvc.isSuccess('Schedule updated');
                    this.submitting.set(false);
                },
                error: (e: HttpErrorResponse) => {
                    if (e.status == 400) {
                        this.msgsSvc.hasErrors(e.error.errors);
                    } else {
                        this.msgsSvc.isError('Unable to update schedule');
                    }
                    this.submitting.set(false);
                },
            });
    }

    public deleteCompany(name: string) {
        this.companySvc
            .deleteCompany(name)
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.router.navigateByUrl('/logout');
                },
                error: (e: HttpErrorResponse) => {
                    if (e.status == 400) {
                        this.msgsSvc.hasErrors(e.error.errors);
                    } else {
                        this.msgsSvc.isError('Unable to delete company');
                    }
                },
            });
    }

    public updateLeaveTypes() {
        this.companySvc.updateLeaveTypes().pipe(takeUntilDestroyed(this.destroyed)).subscribe();
    }

    public addLeaveType() {
        this.leaveTypes.push(this.leaveTypeForm);

        this.companySvc.resetLeaveTypeForm();
    }

    public removeLeaveType(id: number) {}
}
