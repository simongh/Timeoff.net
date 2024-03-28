import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllowanceUsageComponent } from './allowance-usage.component';

describe('AllowanceUsageComponent', () => {
  let component: AllowanceUsageComponent;
  let fixture: ComponentFixture<AllowanceUsageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AllowanceUsageComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AllowanceUsageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
