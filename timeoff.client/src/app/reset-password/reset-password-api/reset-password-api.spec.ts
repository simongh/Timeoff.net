import { TestBed } from '@angular/core/testing';

import { ResetPasswordApi } from './reset-password-api';

describe('ResetPasswordApi', () => {
  let service: ResetPasswordApi;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ResetPasswordApi);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
