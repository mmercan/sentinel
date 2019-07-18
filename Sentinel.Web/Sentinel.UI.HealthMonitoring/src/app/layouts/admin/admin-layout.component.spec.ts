import { CommonModule } from '@angular/common';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { NgbCollapseModule, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SidebarModule } from 'ng-sidebar';
import { SharedModule } from '../../shared/shared.module';
import { AdminLayoutComponent } from './admin-layout.component';

describe('AdminLayoutComponent', () => {
    let component: AdminLayoutComponent;
    let fixture: ComponentFixture<AdminLayoutComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [FormsModule, ReactiveFormsModule, NgbModule.forRoot(),
                SidebarModule,
                RouterModule.forRoot([]), SharedModule.forRoot()],
            declarations: [AdminLayoutComponent],
        })
            .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(AdminLayoutComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should be created', () => {
        expect(component).toBeTruthy();
    });

    it('should set Title', () => {
        expect(component.setTitle('Blah')).toBeUndefined(); //toBeTruthy();
    });

    it('should toogle Sidebar', () => {
        expect(component.toogleSidebar()).toBeUndefined();
    });

    it('should check isOver', () => {
        expect(component.isOver()).toBeDefined();
    });

    it('should add Menu Item', () => {
        expect(component.addMenuItem()).toBeUndefined();
    });

    it('should add Menu Item', () => {
        expect(component.sendnotification('Success', 'Do it', 'test worked')).toBeUndefined();
    });

    // tslint:disable-next-line:no-commented-code
    // it('should submit', () => {
    //   expect(component.onSubmit()).toBeTruthy();
    // });
});
