import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { ImportConfigComponent } from './import-config.component';


describe('ImportConfigComponent', () => {
  let component: ImportConfigComponent;
  let fixture: ComponentFixture<ImportConfigComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [CommonModule, RouterModule.forRoot([]), SharedModule.forRoot()],
      declarations: [ImportConfigComponent],
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImportConfigComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
