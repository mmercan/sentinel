import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SettingsComponent } from './settings/settings.component';
import { ProfileComponent } from './profile/profile.component';
import { NotificationSettingsComponent } from './notification-settings/notification-settings.component';
import { UserRoutingModule } from './user-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { SharedModule } from '../shared/shared.module';
@NgModule({
  imports: [
    CommonModule,
    UserRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    MatSlideToggleModule,
    SharedModule
  ],
  declarations: [SettingsComponent, ProfileComponent, NotificationSettingsComponent]
})
export class UserModule { }
