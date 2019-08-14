import { Directive, EmbeddedViewRef, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { OfflineNotificationService } from './offline-notification.service';

@Directive({
  selector: '[IfOnline]'
})
export class IfOnlineDirective {
  CheckifOffline = false;

  private _context: IfOnlineContext = new IfOnlineContext();
  private _thenTemplateRef: TemplateRef<IfOnlineContext> | null = null;
  private _thenViewRef: EmbeddedViewRef<IfOnlineContext> | null = null;

  constructor(private _viewContainer: ViewContainerRef
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
    this.offlineNotificationService.GetStatus().subscribe(
      result => {
        if (this.CheckifOffline) {
          this._context.$implicit = this._context.ifOnline = result === 'offline';
        } else {
          this._context.$implicit = this._context.ifOnline = result === 'online';
        }
        this._updateView();
      },
      error => { }
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
}

export class IfOnlineContext {
  public $implicit: any = null;
  public ifOnline: any = null;
}
