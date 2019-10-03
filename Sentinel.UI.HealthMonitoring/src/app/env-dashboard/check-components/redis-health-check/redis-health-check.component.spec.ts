import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RedisHealthCheckComponent } from './redis-health-check.component';

describe('RedisHealthCheckComponent', () => {
  let component: RedisHealthCheckComponent;
  let fixture: ComponentFixture<RedisHealthCheckComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RedisHealthCheckComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RedisHealthCheckComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
