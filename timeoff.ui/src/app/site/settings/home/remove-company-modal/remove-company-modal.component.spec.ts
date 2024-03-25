import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RemoveCompanyModalComponent } from './remove-company-modal.component';

describe('RemoveCompanyModalComponent', () => {
  let component: RemoveCompanyModalComponent;
  let fixture: ComponentFixture<RemoveCompanyModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RemoveCompanyModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RemoveCompanyModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
