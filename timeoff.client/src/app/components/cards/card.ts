import { Component } from '@angular/core';

@Component({
  selector: 'ton-card',
  template: '<div class="card"><ng-content/></div>',
  styles: ':host {display:contents}',
})
export class Card {}
