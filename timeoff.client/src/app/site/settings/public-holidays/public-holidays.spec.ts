import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PublicHolidays } from './public-holidays';

describe('PublicHolidays', () => {
  let component: PublicHolidays;
  let fixture: ComponentFixture<PublicHolidays>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PublicHolidays]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PublicHolidays);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
