import { Component, ChangeDetectionStrategy } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';

import { Footer } from './footer/footer';
import { Header } from './header/header';

@Component({
  selector: 'ton-site',
  imports: [RouterOutlet, Header, Footer],
  templateUrl: './site.html',
  changeDetection: ChangeDetectionStrategy.Eager,
  styleUrl: './site.scss'
})
export class Site {

}
