import { TestBed } from '@angular/core/testing';

import { ForgotPasswordApi } from './forgot-password-api';

describe('ForgotPasswordApi', () => {
  let service: ForgotPasswordApi;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ForgotPasswordApi);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
