import { Component } from '@angular/core';
import { SettingsService } from './shared/settings/settings.service';

@Component({
  selector: 'app-root',
  template: '<router-outlet></router-outlet>'
})
export class AppComponent {

  constructor(settingsService: SettingsService) {

  }
}

