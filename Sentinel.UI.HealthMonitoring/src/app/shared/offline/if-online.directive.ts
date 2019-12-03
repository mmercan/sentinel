import { Directive, EmbeddedViewRef, Input, TemplateRef, ViewContainerRef, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { SignalRService } from '../signal-r/signal-r.service';
import { OfflineNotificationService } from './offline-notification.service';

@Directive({
  selector: '[IfOnline]'
})
export class IfOnlineDirective implements OnDestroy {

  CheckifOffline = false;

  private _context: IfOnlineContext = new IfOnlineContext();
  private _thenTemplateRef: TemplateRef<IfOnlineContext> | null = null;
  private _thenViewRef: EmbeddedViewRef<IfOnlineContext> | null = null;
  GetStatusSubscription: Subscription;

  constructor(private _viewContainer: ViewContainerRef, private signalRService: SignalRService
    , templateRef: TemplateRef<IfOnlineContext>, private offlineNotificationService: OfflineNotificationService) {
    this._thenTemplateRef = templateRef;
  }

  @Input()
  set IfOnline(condition: any) {
    if (condition === false || condition === 'false') {
      this.CheckifOffline = true;
    }

    if (this.CheckifOffline) {
      this._context.$implicit = this._context.ifOnline = this.offlineNotificationService.isOffline();
    } else {
      this._context.$implicit = this._context.ifOnline = !this.offlineNotificationService.isOffline();
    }
    if (this.signalRService.signalRConnected) {
      this._context.$implicit = this._context.ifOnline = 'signalRConnected';
    }

    this.signalRService.GetStatus().subscribe((result) => {
      if (this.signalRService.signalRConnected) {
        this._context.$implicit = this._context.ifOnline = 'signalRConnected';
      } else {
        this._context.$implicit = this._context.ifOnline = this.offlineNotificationService.isOffline();
      }
    });

    this.GetStatusSubscription = this.offlineNotificationService.GetStatus().subscribe(
      (result) => {
        if (this.CheckifOffline) {
          this._context.$implicit = this._context.ifOnline = result === 'offline';
        } else {
          this._context.$implicit = this._context.ifOnline = result === 'online';
        }
        this._updateView();
      },
      (error) => { },
    );
    this._updateView();
  }

  private _updateView() {
    if (this._context.$implicit) {
      if (!this._thenViewRef) {
        this._viewContainer.clear();
        if (this._thenTemplateRef) {
          this._thenViewRef = this._viewContainer.createEmbeddedView(this._thenTemplateRef, this._context);
        }
      }
    } else {
      this._viewContainer.clear();
      this._thenViewRef = null;
    }
  }

  ngOnDestroy(): void {
    if (this.GetStatusSubscription) { this.GetStatusSubscription.unsubscribe(); }
  }
}

export class IfOnlineContext {
  public $implicit: any = null;
  public ifOnline: any = null;
}
