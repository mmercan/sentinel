import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PerformanceCounterListComponent } from './performance-counter-list.component';
import { CommonModule } from '@angular/common';
import { NgbCollapseModule } from '@ng-bootstrap/ng-bootstrap';

describe('PerformanceCounterListComponent', () => {
  let component: PerformanceCounterListComponent;
  let fixture: ComponentFixture<PerformanceCounterListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [CommonModule, NgbCollapseModule.forRoot()],
      declarations: [PerformanceCounterListComponent]
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
