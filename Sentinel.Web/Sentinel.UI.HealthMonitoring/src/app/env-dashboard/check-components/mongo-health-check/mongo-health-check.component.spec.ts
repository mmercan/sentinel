import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MongoHealthCheckComponent } from './mongo-health-check.component';

describe('MongoHealthCheckComponent', () => {
  let component: MongoHealthCheckComponent;
  let fixture: ComponentFixture<MongoHealthCheckComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MongoHealthCheckComponent ]
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
