import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OathCallbackComponent } from './oath-callback.component';

describe('OathCallbackComponent', () => {
  let component: OathCallbackComponent;
  let fixture: ComponentFixture<OathCallbackComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OathCallbackComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OathCallbackComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
