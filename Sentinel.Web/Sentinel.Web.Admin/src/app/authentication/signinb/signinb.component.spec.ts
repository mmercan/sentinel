import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SigninbComponent } from './signinb.component';

describe('SigninbComponent', () => {
  let component: SigninbComponent;
  let fixture: ComponentFixture<SigninbComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SigninbComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SigninbComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
