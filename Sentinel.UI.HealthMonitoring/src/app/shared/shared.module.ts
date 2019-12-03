import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule } from '@angular/common/http';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { AppConfig, authenticationType, logLevel } from '../app.config';
import { AccordionAnchorDirective, AccordionDirective, AccordionLinkDirective } from './accordion';
import { AdalGuard } from './authentication/adal-auth/adal.guard';
import { AdalInterceptor } from './authentication/adal-auth/adal.interceptor';
import { AdalService } from './authentication/adal-auth/adal.service';
import { AuthService } from './authentication/auth.service';
import { IfAuthenticatedDirective } from './authentication/if-authenticated/if-authenticated.directive';
import { LocalAuthService } from './authentication/local-auth/local-auth.service';
import { UserAvatarComponent } from './authentication/user-avatar/user-avatar.component';
import { ConfigDataService } from './data-store/config-data/config-data.service';
import { HealthcheckDataStoreSetService } from './data-store/healthcheck-data-store/healthcheck-data-store-set.service';
import { HealthcheckDataStoreService } from './data-store/healthcheck-data-store/healthcheck-data-store.service';
import { ToggleFullscreenDirective } from './fullscreen/toggle-fullscreen.directive';
import { MenuItems } from './menu-items/menu-items';
import { NotificationService } from './notification/notification.service';
import { IfOnlineDirective } from './offline/if-online.directive';
import { IfSignalrDirective } from './offline/if-signalr.directive';
import { OfflineNotificationService } from './offline/offline-notification.service';
import { Ng4FilterPipe } from './pipes/filter/ng4-filter.pipe';
import { SignalRService } from './signal-r/signal-r.service';

@NgModule({
  imports: [HttpClientModule],
  declarations: [AccordionAnchorDirective, AccordionLinkDirective, AccordionDirective, ToggleFullscreenDirective,
    IfAuthenticatedDirective, IfOnlineDirective, UserAvatarComponent, Ng4FilterPipe, IfSignalrDirective],
  exports: [AccordionAnchorDirective, AccordionLinkDirective, AccordionDirective, ToggleFullscreenDirective,
    IfAuthenticatedDirective, IfOnlineDirective, UserAvatarComponent, Ng4FilterPipe, IfSignalrDirective],
  providers: [],
})
export class SharedModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: SharedModule,
      providers: [AppConfig, AdalService, LocalAuthService, AuthService, MenuItems, NotificationService, AdalInterceptor,
        OfflineNotificationService, HealthcheckDataStoreService, HealthcheckDataStoreSetService, AdalGuard, ConfigDataService,
        SignalRService],
    };
  }
}
