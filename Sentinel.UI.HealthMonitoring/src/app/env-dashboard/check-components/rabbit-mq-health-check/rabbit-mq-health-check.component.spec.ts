import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RabbitMqHealthCheckComponent } from './rabbit-mq-health-check.component';
import { CommonModule } from '@angular/common';
import { NgbCollapseModule } from '@ng-bootstrap/ng-bootstrap';

describe('RabbitMqHealthCheckComponent', () => {
  let component: RabbitMqHealthCheckComponent;
  let fixture: ComponentFixture<RabbitMqHealthCheckComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [CommonModule, NgbCollapseModule.forRoot()],
      declarations: [RabbitMqHealthCheckComponent]
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
