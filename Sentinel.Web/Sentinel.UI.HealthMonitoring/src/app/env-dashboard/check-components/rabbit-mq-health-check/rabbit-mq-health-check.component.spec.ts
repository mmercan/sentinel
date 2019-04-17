import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RabbitMqHealthCheckComponent } from './rabbit-mq-health-check.component';

describe('RabbitMqHealthCheckComponent', () => {
  let component: RabbitMqHealthCheckComponent;
  let fixture: ComponentFixture<RabbitMqHealthCheckComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RabbitMqHealthCheckComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RabbitMqHealthCheckComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
