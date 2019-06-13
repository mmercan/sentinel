import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IsAliveRequestFailedCheckComponent } from './is-alive-request-failed-check.component';
import { CommonModule } from '@angular/common';
import { NgbCollapseModule } from '@ng-bootstrap/ng-bootstrap';

describe('IsAliveRequestFailedCheckComponent', () => {
  let component: IsAliveRequestFailedCheckComponent;
  let fixture: ComponentFixture<IsAliveRequestFailedCheckComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [CommonModule, NgbCollapseModule.forRoot()],
      declarations: [IsAliveRequestFailedCheckComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IsAliveRequestFailedCheckComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
