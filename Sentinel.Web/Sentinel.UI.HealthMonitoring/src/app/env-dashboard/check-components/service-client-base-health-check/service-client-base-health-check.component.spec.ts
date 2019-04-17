import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ServiceClientBaseHealthCheckComponent } from './service-client-base-health-check.component';

describe('ServiceClientBaseHealthCheckComponent', () => {
  let component: ServiceClientBaseHealthCheckComponent;
  let fixture: ComponentFixture<ServiceClientBaseHealthCheckComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ServiceClientBaseHealthCheckComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ServiceClientBaseHealthCheckComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
