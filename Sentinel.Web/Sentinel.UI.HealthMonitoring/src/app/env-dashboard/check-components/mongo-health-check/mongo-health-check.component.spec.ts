import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MongoHealthCheckComponent } from './mongo-health-check.component';
import { CommonModule } from '@angular/common';
import { NgbCollapseModule } from '@ng-bootstrap/ng-bootstrap';

describe('MongoHealthCheckComponent', () => {
  let component: MongoHealthCheckComponent;
  let fixture: ComponentFixture<MongoHealthCheckComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [CommonModule, NgbCollapseModule.forRoot()],
      declarations: [MongoHealthCheckComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MongoHealthCheckComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
