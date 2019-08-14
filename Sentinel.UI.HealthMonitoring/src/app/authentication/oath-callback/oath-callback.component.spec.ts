import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OathCallbackComponent } from './oath-callback.component';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';

describe('OathCallbackComponent', () => {
  let component: OathCallbackComponent;
  let fixture: ComponentFixture<OathCallbackComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [CommonModule, RouterModule.forRoot([]), SharedModule.forRoot()],
      declarations: [OathCallbackComponent]
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
