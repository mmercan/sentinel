import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {NotificationSettingsComponent} from './notification-settings/notification-settings.component';
import {ProfileComponent} from './profile/profile.component';
import {SettingsComponent} from './settings/settings.component';
const routes: Routes = [{
  path: '',
  component: ProfileComponent,
  data: {
    heading: 'Profile'
  }
},
{
  path: 'settings',
  component: SettingsComponent,
  data: {
    heading: 'settings'
  }
},
{
  path: 'notificationsettings',
  component: NotificationSettingsComponent,
  data: {
    heading: 'Notification Settings'
  }
}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
