import { Component, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'ton-card',
  template: '<div class="card"><ng-content/></div>',
  changeDetection: ChangeDetectionStrategy.Eager,
  styles: ':host {display:contents}',
})
export class Card {}
