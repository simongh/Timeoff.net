import { Component, computed, inject, linkedSignal, numberAttribute, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { FormField } from '@angular/forms/signals';
import { RouterLink } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faChevronRight, faChevronLeft } from '@fortawesome/free-solid-svg-icons';
import { NgbInputDatepicker } from '@ng-bootstrap/ng-bootstrap';
import { injectQueryParams } from 'ngxtension/inject-query-params';

import { Calendar } from '@components/calendar/calendar';
import { Card } from '@components/cards';
import { Messages } from '@components/messages/messages';
import { MessagesService } from '@components/messages/messages.service';
import { PageHeader } from '@components/page-header/page-header';

import { AuthService } from '@app-types/auth/auth.service';

import { AddModel, PublicHolidaysApi } from './public-holidays-api/public-holidays-api';

@Component({
  selector: 'ton-public-holidays',
  imports: [PageHeader, Card, RouterLink, FormsModule, Calendar, FontAwesomeModule, FormField, Messages, NgbInputDatepicker],
  templateUrl: './public-holidays.html',
  styleUrl: './public-holidays.scss',
})
export class PublicHolidays {
  readonly #holidayApi = inject(PublicHolidaysApi);

  readonly #msgsSvc = inject(MessagesService);

  readonly #auth = inject(AuthService);

  protected readonly company = this.#auth.companyName;

  protected readonly dateFormat = this.#auth.dateFormat;

  protected readonly currentYear = injectQueryParams((p) =>
    numberAttribute(p['year'] ?? new Date().getFullYear()),
  );

  protected readonly nextYear = computed(() => this.currentYear() + 1);

  protected readonly lastYear = computed(() => this.currentYear() - 1);

  protected readonly start = computed(() => `${this.currentYear()}-01-01`);

  protected readonly holidays = this.#holidayApi.getHolidays(() => this.currentYear());

  protected readonly model = linkedSignal(() =>
    (this.holidays?.value() ?? []).map(
      (h) =>
        ({
          id: h.id,
          date: h.date,
          name: h.name,
        }) as AddModel,
    ),
  );

  protected readonly faChevronLeft = faChevronLeft;

  protected readonly faChevronRight = faChevronRight;

  protected readonly form = this.#holidayApi.createEditForm(this.model);

  protected save() {
    if (this.form().invalid())
      return;

    this.#holidayApi.update.load({
      payload: [this.model()],
      subscriber: {
        next: () => {
          this.#msgsSvc.addSuccess('Public holidays updated');
        },
        error: () => {
          this.#msgsSvc.addError('Unable to update public holidays');
        },
      },
    });
  }

  protected clear() {
    this.#msgsSvc.clear();
  }
}
