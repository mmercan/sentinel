import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SystemInfoHealthChecksComponent } from './system-info-health-checks.component';
import { CommonModule } from '@angular/common';
import { NgbCollapseModule } from '@ng-bootstrap/ng-bootstrap';

describe('SystemInfoHealthChecksComponent', () => {
  let component: SystemInfoHealthChecksComponent;
  let fixture: ComponentFixture<SystemInfoHealthChecksComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [CommonModule, NgbCollapseModule.forRoot()],
      declarations: [SystemInfoHealthChecksComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SystemInfoHealthChecksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
