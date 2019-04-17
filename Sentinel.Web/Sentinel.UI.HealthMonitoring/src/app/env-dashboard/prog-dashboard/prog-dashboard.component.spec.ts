import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProgDashboardComponent } from './prog-dashboard.component';

describe('ProgDashboardComponent', () => {
  let component: ProgDashboardComponent;
  let fixture: ComponentFixture<ProgDashboardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProgDashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
