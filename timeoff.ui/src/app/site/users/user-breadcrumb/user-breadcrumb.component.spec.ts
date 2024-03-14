import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserBreadcrumbComponent } from './user-breadcrumb.component';

describe('UserBreadcrumbComponent', () => {
  let component: UserBreadcrumbComponent;
  let fixture: ComponentFixture<UserBreadcrumbComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserBreadcrumbComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(UserBreadcrumbComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
