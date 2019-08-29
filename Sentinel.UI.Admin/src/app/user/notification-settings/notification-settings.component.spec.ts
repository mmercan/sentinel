import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NotificationSettingsComponent } from './notification-settings.component';

import { MatSlideToggleModule } from '@angular/material';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { SharedModule } from '../../shared/shared.module';
import { AppConfig } from '../../app.config';



describe('NotificationSettingsComponent', () => {
  let component: NotificationSettingsComponent;
  let fixture: ComponentFixture<NotificationSettingsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [FormsModule, ReactiveFormsModule, SharedModule.forRoot(), MatSlideToggleModule],
      declarations: [NotificationSettingsComponent],
      providers: [AppConfig]
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
