import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidatorMessage } from './validator-message';

describe('ValidatorMessage', () => {
  let component: ValidatorMessage;
  let fixture: ComponentFixture<ValidatorMessage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ValidatorMessage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ValidatorMessage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
