import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PerformanceCounterListComponent } from './performance-counter-list.component';

describe('PerformanceCounterListComponent', () => {
  let component: PerformanceCounterListComponent;
  let fixture: ComponentFixture<PerformanceCounterListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PerformanceCounterListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PerformanceCounterListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
