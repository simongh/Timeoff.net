import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CarryOver } from './carry-over';

describe('CarryOver', () => {
  let component: CarryOver;
  let fixture: ComponentFixture<CarryOver>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CarryOver]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CarryOver);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
