import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NotificationSettingsComponent } from './notification-settings.component';

import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { MatSlideToggleModule } from '@angular/material';
import { AppConfig, authenticationType, logLevel } from '../../app.config';
import { Notification, NotificationService } from '../../shared/notification/notification.service';
import { AdalService } from '../../shared/authentication/adal-auth/adal.service';


describe('NotificationSettingsComponent', () => {
  let component: NotificationSettingsComponent;
  let fixture: ComponentFixture<NotificationSettingsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [CommonModule, HttpClientModule, FormsModule, MatSlideToggleModule],
      declarations: [NotificationSettingsComponent],
      providers: [NotificationService, AdalService, AppConfig]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NotificationSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
