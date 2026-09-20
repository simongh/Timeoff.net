import { Component, ChangeDetectionStrategy } from '@angular/core';

import { PageHeader } from '@components/page-header/page-header';

@Component({
  selector: 'ton-home',
  imports: [PageHeader],
  templateUrl: './home.html',
  changeDetection: ChangeDetectionStrategy.Eager,
  styleUrl: './home.scss'
})
export class Home {

}
