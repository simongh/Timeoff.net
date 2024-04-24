import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RemoveCompanyComponent } from './remove-company.component';

describe('RemoveCompanyComponent', () => {
    let component: RemoveCompanyComponent;
    let fixture: ComponentFixture<RemoveCompanyComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [RemoveCompanyComponent],
        }).compileComponents();

        fixture = TestBed.createComponent(RemoveCompanyComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
