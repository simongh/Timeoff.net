import { Component, computed, inject, numberAttribute, signal } from '@angular/core';
import { derivedAsync } from 'ngxtension/derived-async';
import { injectQueryParams } from 'ngxtension/inject-query-params';

import { Card } from '@components/cards';
import { PageHeader } from '@components/page-header/page-header';

import { AuthService } from '@app-types/auth/auth.service';

import { PublicHolidaysApi } from './public-holidays-api/public-holidays-api';

@Component({
  selector: 'ton-public-holidays',
  imports: [PageHeader, Card],
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

  protected readonly holidays = this.#holidayApi.getHolidays(() => this.currentYear());
}
