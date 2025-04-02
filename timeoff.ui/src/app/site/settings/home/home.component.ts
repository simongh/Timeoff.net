import { CommonModule } from '@angular/common';
import { Component, DestroyRef, OnInit, computed, inject, signal } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { combineLatest } from 'rxjs';

import { dateFormats } from '@models/date-formats';

import { FlashComponent } from '@components/flash/flash.component';
import { ScheduleComponent } from '@components/schedule/schedule.component';

import { CompanyService } from '@services/company/company.service';
import { MessagesService } from '@services/messages/messages.service';
import { TimeZoneModel } from '@services/company/time-zone.model';
import { Country } from '@services/company/country.model';

import { RemoveCompanyComponent } from './remove-company/remove-company.component';
import { LeaveTypesComponent } from './leave-types/leave-types.component';
import { derivedAsync } from 'ngxtension/derived-async';

@Component({
    selector: 'app-home',
    providers: [CompanyService],
    templateUrl: './home.component.html',
    styleUrl: './home.component.scss',
    imports: [
        CommonModule,
        ReactiveFormsModule,
        RouterLink,
        FlashComponent,
        ScheduleComponent,
        RemoveCompanyComponent,
        LeaveTypesComponent,
    ]
})
export class HomeComponent implements OnInit {
    readonly #destroyed = inject(DestroyRef);

    readonly #companySvc = inject(CompanyService);

    readonly #messagesSvc = inject(MessagesService);

    protected readonly countries = derivedAsync(()=>this.#companySvc.countries());

    protected readonly dateFormats = signal(dateFormats);

    protected readonly timeZones = derivedAsync(()=> this.#companySvc.timeZones());

    protected readonly carryOverDays = computed(() => {
        const days = Array<number>();
        for (let i = 0; i < 22; i++) {
            days.push(i);
        }
        days.push(1000);

        return days;
    });

    protected readonly currentYear = computed(() => new Date().getFullYear());

    protected readonly lastYear = computed(() => this.currentYear() - 1);

    protected get companyName() {
        return this.#companySvc.settingsForm.value.name;
    }

    public get settingsForm() {
        return this.#companySvc.settingsForm;
    }

    public get days() {
        return this.#companySvc.settingsForm.controls.schedule;
    }

    protected readonly submitting = signal(false);

    public ngOnInit(): void {
        this.#companySvc.getSettings()
            .pipe(takeUntilDestroyed(this.#destroyed))
            .subscribe((data) => {

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

                this.#companySvc.fillLeaveTypes(data.leaveTypes);
            });
    }

    public save() {
        this.settingsForm.markAllAsTouched();

        if (this.settingsForm.invalid) {
            return;
        }

        this.submitting.set(true);

        this.#companySvc
            .saveSettings()
            .pipe(takeUntilDestroyed(this.#destroyed))
            .subscribe({
                next: () => {
                    this.submitting.set(false);
                    this.#messagesSvc.isSuccess('Company details updated');
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

        this.#companySvc
            .saveSchedule()
            .pipe(takeUntilDestroyed(this.#destroyed))
            .subscribe({
                next: () => {
                    this.#messagesSvc.isSuccess('Schedule updated');
                    this.submitting.set(false);
                },
            });
    }
}
