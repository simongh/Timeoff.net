import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllowanceBreakdownComponent } from './allowance-breakdown.component';

describe('AllowanceBreakdownComponent', () => {
  let component: AllowanceBreakdownComponent;
  let fixture: ComponentFixture<AllowanceBreakdownComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AllowanceBreakdownComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AllowanceBreakdownComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
