import { Component, computed, inject, numberAttribute } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { FormField } from '@angular/forms/signals';
import { RouterLink } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faChevronRight, faChevronLeft} from '@fortawesome/free-solid-svg-icons'
import { injectQueryParams } from 'ngxtension/inject-query-params';

import { Calendar } from '@components/calendar/calendar';
import { Card } from '@components/cards';
import { PageHeader } from '@components/page-header/page-header';

import { AuthService } from '@app-types/auth/auth.service';

import { PublicHolidaysApi } from './public-holidays-api/public-holidays-api';

@Component({
  selector: 'ton-public-holidays',
  imports: [PageHeader, Card, RouterLink, FormsModule, Calendar, FontAwesomeModule, FormField],
  templateUrl: './public-holidays.html',
  styleUrl: './public-holidays.scss',
})
export class PublicHolidays {
  readonly #holidayApi = inject(PublicHolidaysApi);

  protected readonly company = inject(AuthService).companyName;

  protected readonly currentYear = injectQueryParams((p) =>
    numberAttribute(p['year'] ?? new Date().getFullYear()),
  );

  protected readonly nextYear = computed(() => this.currentYear() + 1);

  protected readonly lastYear = computed(() => this.currentYear() - 1);

  protected readonly start = computed(()=>`${this.currentYear()}-01-01`);

  protected readonly holidays = this.#holidayApi.getHolidays(() => this.currentYear());

  protected readonly faChevronLeft = faChevronLeft;

  protected readonly faChevronRight = faChevronRight;

  protected readonly form = this.#holidayApi.createEditForm(this.holidays.value() ?? []);
}
