import { NgModule, ModuleWithProviders } from '@angular/core';
import { HttpModule } from '@angular/http';
import { FormsModule } from '@angular/forms';

import { MenuItems } from './menu-items/menu-items';
import { AccordionAnchorDirective, AccordionLinkDirective, AccordionDirective } from './accordion';
import { ToggleFullscreenDirective } from './fullscreen/toggle-fullscreen.directive';
import { NotificationService } from './notification/notification.service';
import { AppConfig, authenticationType, logLevel } from '../app.config';

import { AdalInterceptor } from './authentication/adal-auth/adal.interceptor';
import { AdalService } from './authentication/adal-auth/adal.service';
import { AdalGuard } from './authentication/adal-auth/adal.guard';
import { LocalAuthService } from './authentication/local-auth/local-auth.service';
import { AuthService } from './authentication/auth.service';
import { IfAuthenticatedDirective } from './authentication/if-authenticated/if-authenticated.directive';
import { UserAvatarComponent } from './authentication/user-avatar/user-avatar.component';

import { IfOnlineDirective } from './offline/if-online.directive';
import { OfflineNotificationService } from './offline/offline-notification.service';


import { Ng4FilterPipe } from './pipes/filter/ng4-filter.pipe';

import { ProductDataStoreService } from './data-store/product-data-store/product-data-store.service';
import { StorageComponent } from './storage/storage.component';

import { ProductCRUDService } from './generated/api.client.generated';
import { SettingsService } from './settings/settings.service';
@NgModule({
  imports: [HttpModule, FormsModule],
  declarations: [AccordionAnchorDirective, AccordionLinkDirective, AccordionDirective, ToggleFullscreenDirective,
    IfAuthenticatedDirective, IfOnlineDirective, UserAvatarComponent, Ng4FilterPipe, StorageComponent],
  exports: [AccordionAnchorDirective, AccordionLinkDirective, AccordionDirective, ToggleFullscreenDirective,
    IfAuthenticatedDirective, IfOnlineDirective, UserAvatarComponent, Ng4FilterPipe, StorageComponent],
  providers: [SettingsService]
})
export class SharedModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: SharedModule,
      providers: [AppConfig, AdalService, LocalAuthService, AuthService, MenuItems, NotificationService, AdalInterceptor,
        OfflineNotificationService, AdalGuard, ProductDataStoreService, ProductCRUDService, SettingsService]
    };
  }
}
