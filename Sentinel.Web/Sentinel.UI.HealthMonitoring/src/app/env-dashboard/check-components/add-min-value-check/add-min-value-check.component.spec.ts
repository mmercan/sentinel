import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddMinValueCheckComponent } from './add-min-value-check.component';

describe('AddMinValueCheckComponent', () => {
  let component: AddMinValueCheckComponent;
  let fixture: ComponentFixture<AddMinValueCheckComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddMinValueCheckComponent ]
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
