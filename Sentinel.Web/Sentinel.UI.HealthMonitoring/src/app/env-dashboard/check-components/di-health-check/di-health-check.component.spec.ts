import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DiHealthCheckComponent } from './di-health-check.component';

describe('DiHealthCheckComponent', () => {
  let component: DiHealthCheckComponent;
  let fixture: ComponentFixture<DiHealthCheckComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DiHealthCheckComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DiHealthCheckComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
