import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProcessListHealthChecksComponent } from './process-list-health-checks.component';

describe('ProcessListHealthChecksComponent', () => {
  let component: ProcessListHealthChecksComponent;
  let fixture: ComponentFixture<ProcessListHealthChecksComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProcessListHealthChecksComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProcessListHealthChecksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
