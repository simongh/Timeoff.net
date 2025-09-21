import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

import { Country } from './country';

@Injectable({
  providedIn: 'root',
})
export class SiteApi {
  readonly #client = inject(HttpClient);
}
