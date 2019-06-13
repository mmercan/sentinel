import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DbConnectionHealthChecksComponent } from './db-connection-health-checks.component';
import { CommonModule } from '@angular/common';
import { NgbCollapseModule } from '@ng-bootstrap/ng-bootstrap';

describe('DbConnectionHealthChecksComponent', () => {
  let component: DbConnectionHealthChecksComponent;
  let fixture: ComponentFixture<DbConnectionHealthChecksComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [CommonModule, NgbCollapseModule.forRoot()],
      declarations: [DbConnectionHealthChecksComponent]
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
