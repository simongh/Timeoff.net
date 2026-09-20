import { Component, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'ton-page-header',
  imports: [],
  template: `<div class="page-header">
    <div class="container-xl">
      <div class="row align-items-center">
        <div class="col"><ng-content /></div>
      </div>
    </div>
  </div>`,
  changeDetection: ChangeDetectionStrategy.Eager,
  styleUrl: './page-header.scss',
})
export class PageHeader {}
