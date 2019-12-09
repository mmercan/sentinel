import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserNotAuthenticatedComponent } from './user-not-authenticated.component';

describe('UserNotAuthenticatedComponent', () => {
  let component: UserNotAuthenticatedComponent;
  let fixture: ComponentFixture<UserNotAuthenticatedComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserNotAuthenticatedComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserNotAuthenticatedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
