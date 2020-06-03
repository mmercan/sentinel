import { NgModule, ModuleWithProviders, ANALYZE_FOR_ENTRY_COMPONENTS } from '@angular/core';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { DragulaModule } from 'ng2-dragula';
import { CommonModule } from '@angular/common';

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
import { ImageClassfierService } from './ai/image-classfier.service';


import { IfOnlineDirective } from './offline/if-online.directive';
import { OfflineNotificationService } from './offline/offline-notification.service';


import { Ng4FilterPipe } from './pipes/filter/ng4-filter.pipe';

import { ProductDataStoreService } from './data-store/product-data-store/product-data-store.service';
import { StorageComponent } from './storage/storage.component';

import { ProductCRUDService } from './generated/api.client.generated';
import { SettingsService } from './settings/settings.service';
import { SettingsComponent } from './settings/settings.component';
import { TestComponent } from './test-tools/test/test.component';



@NgModule({
  imports: [CommonModule, HttpClientModule, FormsModule, DragulaModule],
  declarations: [AccordionAnchorDirective, AccordionLinkDirective, AccordionDirective, ToggleFullscreenDirective,
    IfAuthenticatedDirective, IfOnlineDirective, UserAvatarComponent, Ng4FilterPipe, StorageComponent, SettingsComponent, TestComponent],
  exports: [AccordionAnchorDirective, AccordionLinkDirective, AccordionDirective, ToggleFullscreenDirective,
    IfAuthenticatedDirective, IfOnlineDirective, UserAvatarComponent, Ng4FilterPipe, StorageComponent, DragulaModule],
  providers: [SettingsService]
})
export class SharedModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: SharedModule,
      providers: [AppConfig, AdalService, LocalAuthService, AuthService, MenuItems, NotificationService, AdalInterceptor,
        OfflineNotificationService, AdalGuard, ProductDataStoreService, ProductCRUDService, SettingsService, ImageClassfierService]
    };
  }
}
