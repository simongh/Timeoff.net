import { Component, computed, effect, inject, OnInit, signal } from '@angular/core';
import { NgbAlert } from '@ng-bootstrap/ng-bootstrap';

import { MessagesService } from './messages.service';

@Component({
  selector: 'ton-messages',
  imports: [NgbAlert],
  template: `@if (hasMessage()) { 
    <ngb-alert [type]="type()" [dismissible]="false">{{ text() }}</ngb-alert>
    }`,
})
export class Messages {
  readonly #messageSvc = inject(MessagesService);

  protected readonly type = computed(()=>this.#messageSvc.message()?.type ?? '');

  protected readonly text = computed(()=>this.#messageSvc.message()?.text ?? '');

  protected readonly hasMessage = computed(() => this.type() !== '');
}
