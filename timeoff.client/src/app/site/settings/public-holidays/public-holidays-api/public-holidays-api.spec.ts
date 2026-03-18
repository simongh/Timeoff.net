import { TestBed } from '@angular/core/testing';

import { PublicHolidaysApi } from './public-holidays-api';

describe('PublicHolidaysApi', () => {
  let service: PublicHolidaysApi;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PublicHolidaysApi);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
