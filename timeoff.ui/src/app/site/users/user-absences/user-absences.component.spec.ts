import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserAbsencesComponent } from './user-absences.component';

describe('UserAbsencesComponent', () => {
  let component: UserAbsencesComponent;
  let fixture: ComponentFixture<UserAbsencesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserAbsencesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(UserAbsencesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
