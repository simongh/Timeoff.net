import { TestBed } from '@angular/core/testing';

import { AllowanceUsageService } from './allowance-usage.service';

describe('AllowanceUsageService', () => {
  let service: AllowanceUsageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AllowanceUsageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
