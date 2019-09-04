import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { UserAvatarComponent } from './user-avatar.component';

import { AppConfig } from '../../../app.config';
import { AdalService } from '../../authentication/adal-auth/adal.service'
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NotificationService } from '../../notification/notification.service';

describe('UserAvatarComponent', () => {
  let component: UserAvatarComponent;
  let fixture: ComponentFixture<UserAvatarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      declarations: [UserAvatarComponent],
      providers: [AppConfig, NotificationService, AdalService],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserAvatarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
