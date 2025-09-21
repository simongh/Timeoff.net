import { TestBed } from '@angular/core/testing';

import { SiteApi } from './site-api';

describe('SiteApi', () => {
  let service: SiteApi;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SiteApi);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
