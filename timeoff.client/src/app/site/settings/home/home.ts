import { Component } from '@angular/core';

import { Card } from '@components/cards';
import { PageHeader } from '@components/page-header/page-header';

import { Backup } from './backup/backup';
import { CarryOver } from './carry-over/carry-over';
import { Schedule } from './schedule/schedule';

@Component({
  selector: 'ton-home',
  imports: [PageHeader, Card, Backup, Schedule, CarryOver],
  templateUrl: './home.html',
  styleUrl: './home.scss'
})
export class Home {

}
