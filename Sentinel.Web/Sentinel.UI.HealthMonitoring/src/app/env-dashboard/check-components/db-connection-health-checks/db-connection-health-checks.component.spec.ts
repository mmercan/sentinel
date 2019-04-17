import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DbConnectionHealthChecksComponent } from './db-connection-health-checks.component';

describe('DbConnectionHealthChecksComponent', () => {
  let component: DbConnectionHealthChecksComponent;
  let fixture: ComponentFixture<DbConnectionHealthChecksComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DbConnectionHealthChecksComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DbConnectionHealthChecksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
