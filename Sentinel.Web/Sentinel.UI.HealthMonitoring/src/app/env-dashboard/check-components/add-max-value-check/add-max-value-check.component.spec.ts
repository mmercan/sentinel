import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddMaxValueCheckComponent } from './add-max-value-check.component';

describe('AddMaxValueCheckComponent', () => {
  let component: AddMaxValueCheckComponent;
  let fixture: ComponentFixture<AddMaxValueCheckComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddMaxValueCheckComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddMaxValueCheckComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
