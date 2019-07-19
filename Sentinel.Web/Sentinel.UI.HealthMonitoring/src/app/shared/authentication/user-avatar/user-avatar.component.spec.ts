import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { UserAvatarComponent } from './user-avatar.component';


import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppConfig, authenticationType, logLevel } from '../../../app.config';
import { NotificationService } from '../../notification/notification.service';
import { AdalService } from '../../authentication/adal-auth/adal.service';

describe('UserAvatarComponent', () => {
  let component: UserAvatarComponent;
  let fixture: ComponentFixture<UserAvatarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [CommonModule, HttpClientModule, RouterModule.forRoot([])],
      declarations: [UserAvatarComponent],
      providers: [AppConfig, NotificationService, AdalService]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserAvatarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should create avatar', () => {
    const canvas = document.createElement('canvas');
    // canvas: HTMLCanvasElement = document.getElementById('canvas') as HTMLCanvasElement;
    expect(component.createLetterAvatar('Matt Mercan', canvas, 32)).toBeUndefined();
  });

  it('should create avatar', () => {
    expect(component.ngAfterViewInit()).toBeUndefined();
  });

});
