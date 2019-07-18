import { CommonModule } from '@angular/common';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SharedModule } from '../shared/shared.module';
import { DocsComponent } from './docs.component';

describe('DocsComponent', () => {
    let component: DocsComponent;
    let fixture: ComponentFixture<DocsComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [CommonModule, FormsModule, ReactiveFormsModule, NgbModule.forRoot(),
                RouterModule.forRoot([]), SharedModule.forRoot()],
            declarations: [DocsComponent],
        })
            .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(DocsComponent);
        component = fixture.componentInstance;

        fixture.detectChanges();
    });

    it('should be created', () => {
        expect(component).toBeTruthy();
    });

    // tslint:disable-next-line:no-commented-code
    // it('should submit', () => {
    //   expect(component.onSubmit()).toBeTruthy();
    // });
});
