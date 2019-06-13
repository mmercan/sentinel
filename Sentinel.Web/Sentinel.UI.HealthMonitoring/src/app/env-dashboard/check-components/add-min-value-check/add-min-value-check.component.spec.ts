import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddMinValueCheckComponent } from './add-min-value-check.component';
import { CommonModule } from '@angular/common';
import { NgbCollapseModule } from '@ng-bootstrap/ng-bootstrap';

describe('AddMinValueCheckComponent', () => {
  let component: AddMinValueCheckComponent;
  let fixture: ComponentFixture<AddMinValueCheckComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [CommonModule, NgbCollapseModule.forRoot()],
      declarations: [AddMinValueCheckComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddMinValueCheckComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
