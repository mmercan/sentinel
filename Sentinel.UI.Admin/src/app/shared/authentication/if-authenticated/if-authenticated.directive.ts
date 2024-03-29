import { Directive, EmbeddedViewRef, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { AuthService } from '../auth.service';

@Directive({
  selector: '[ifAuthenticated]'
})
export class IfAuthenticatedDirective {
  private _context: IfAuthenticatedContext = new IfAuthenticatedContext();
  private _thenTemplateRef: TemplateRef<IfAuthenticatedContext> | null = null;
  private _thenViewRef: EmbeddedViewRef<IfAuthenticatedContext> | null = null;
  private CheckifUnauthenticated = false;

  constructor(private authService: AuthService, private _viewContainer: ViewContainerRef
    , templateRef: TemplateRef<IfAuthenticatedContext>) {
    this._thenTemplateRef = templateRef;

  }

  @Input()
  set ifAuthenticated(condition: any) {
    if (condition === false || condition === 'false') {
      this.CheckifUnauthenticated = true;
    }

    if (this.CheckifUnauthenticated) {
      this._context.$implicit = this._context.ifAuthenticated = !this.authService.checkLogin();
    } else {
      this._context.$implicit = this._context.ifAuthenticated = this.authService.checkLogin();
    }
    this.authService.checkLogin().subscribe(
      result => {
        if (this.CheckifUnauthenticated) {
          this._context.$implicit = this._context.ifAuthenticated = !result;
        } else {
          this._context.$implicit = this._context.ifAuthenticated = result;
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

export class IfAuthenticatedContext {
  public $implicit: any = null;
  public ifAuthenticated: any = null;
}
