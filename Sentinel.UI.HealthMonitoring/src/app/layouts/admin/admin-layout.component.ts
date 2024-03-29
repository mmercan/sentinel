import { AfterViewInit, Component, NgZone, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';
import { AppConfig, authenticationType } from '../../app.config';
import { AuthService } from '../../shared/authentication/auth.service';
import { ConfigDataService } from '../../shared/data-store/config-data/config-data.service';
import { MenuItems } from '../../shared/menu-items/menu-items';
import { Notification, NotificationService } from '../../shared/notification/notification.service';
import { OfflineNotificationService } from '../../shared/offline/offline-notification.service';
import { SignalRService } from '../../shared/signal-r/signal-r.service';
const SMALL_WIDTH_BREAKPOINT = 991;

export interface IOptions {
  heading?: string;
  removeFooter?: boolean;
  mapHeader?: boolean;
}

@Component({
  selector: 'app-layout',
  templateUrl: './admin-layout.component.html',
  styleUrls: ['./admin-layout.component.scss'],
})
export class AdminLayoutComponent implements OnInit, OnDestroy, AfterViewInit {

  private _router: Subscription;
  private mediaMatcher: MediaQueryList = matchMedia(`(max-width: ${SMALL_WIDTH_BREAKPOINT}px)`);

  currentLang = 'en';
  options: IOptions;
  theme = 'dark';
  showSettings = false;
  isDocked = false;
  isBoxed = false;
  isOpened = true;
  mode = 'push';
  _mode = this.mode;
  _autoCollapseWidth = 991;
  width = window.innerWidth;
  isOffline = false;
  isLoggedin = false;
  counterNumber = 0;
  swipeCount = 0;
  idx: any;
  activeNotifications: Notification[] = [];
  notificationCount = 0;
  @ViewChild('sidebar', { static: true }) sidebar;
  @ViewChild('notificationDrop', { static: true }) notificationDrop;
  params: any;
  programName: any;
  envName: any;
  notificationServiceSubscription: Subscription;

  constructor(
    public menuItems: MenuItems,
    private router: Router,
    private route: ActivatedRoute,
    private modalService: NgbModal,
    private titleService: Title,
    private zone: NgZone,
    private authService: AuthService,
    public appConfig: AppConfig,
    private notificationService: NotificationService,
    private configDataService: ConfigDataService,
    private signalRService: SignalRService,
  ) {
    this.mediaMatcher.addListener(() => zone.run(() => this.mediaMatcher = matchMedia(`(max-width: ${SMALL_WIDTH_BREAKPOINT}px)`)));

    this.notificationServiceSubscription = this.notificationService.dataset.subscribe((result) => {
      if (result && result.length) {
        this.notificationCount = result.length;
        if (this.notificationDrop) {
          setTimeout(() => { this.notificationDrop.open(); }, 500);
        }
      } else {
        this.notificationCount = 0;
      }
    });
    signalRService.startConnection();
  }

  ngOnInit(): void {

    if (this.isOver()) {
      this._mode = 'over';
      this.isOpened = false;
    }

    this._router = this.router.events.pipe(filter((event) => event instanceof NavigationEnd)).subscribe((event: NavigationEnd) => {
      // Scroll to top on view load
      document.querySelector('.main-content').scrollTop = 0;
      this.runOnRouteChange();
      this.appConfig.navigatingto = null;
    });

    this.configDataService.getMenuItemsAsync().subscribe((items) => {
      if (items && items.length && items.length > 0) {
        for (const menu of items) {
          this.menuItems.add(menu);
        }
      }
      this.runOnRouteChange();
    });
  }

  ngAfterViewInit(): void {
    setTimeout((_: any) => this.runOnRouteChange());
  }

  runOnRouteChange(): void {
    if (this.isOver() || this.router.url === '/maps/fullscreen') {
      this.isOpened = false;
    }

    this.route.children.forEach((route: ActivatedRoute) => {
      let activeRoute: ActivatedRoute = route;
      while (activeRoute.firstChild) {
        activeRoute = activeRoute.firstChild;
      }
      this.options = activeRoute.snapshot.data;
      this.params = activeRoute.snapshot.params;
    });

    if (this.options) {
      if (this.options.hasOwnProperty('heading')) {
        this.setTitle(this.appConfig.config.title);
      }
      if (this.params.hasOwnProperty('programname')) {
        this.programName = this.params['programname'];
      }
      if (this.params.hasOwnProperty('envname')) {
        this.envName = this.params['envname'];
      }
    }
  }

  setTitle(newTitle: string) {
    this.titleService.setTitle(this.appConfig.config.title);
  }

  toogleSidebar(): void {
    this.isOpened = !this.isOpened;
  }

  isOver(): boolean {
    return window.matchMedia(`(max-width: 991px)`).matches;
  }

  openSearch(search) {
    this.modalService.open(search, { windowClass: 'search', backdrop: false });
  }

  addMenuItem(): void {
    this.menuItems.add({
      state: 'menu',
      name: 'MENU',
      type: 'sub',
      icon: 'basic-webpage-txt',
      children: [
        { state: 'menu', name: 'MENU' },
        { state: 'menu', name: 'MENU' },
      ],
    });
  }

  signout() {
    this.authService.logout();
  }
  sendnotification(type, title, description) {
    switch (type) {
      case 'Success':
        this.notificationService.showSuccess(title, description, false);
        break;
      case 'Error':
        this.notificationService.showError(title, description, true);
        break;
      case 'Warning':
        this.notificationService.showWarning(title, description, true);
        break;
      case 'Info':
        this.notificationService.showInfo(title, description, false);
        break;
      default:
        break;
    }
  }
  notificationOpened(event: any) {
    if (event) {
      this.activeNotifications = this.notificationService.snapshot();
    }
  }
  removeNotification(notification: Notification) {
    this.notificationService.remove(notification);
  }

  ngOnDestroy() {
    if (this._router) { this._router.unsubscribe(); }
    if (this.notificationServiceSubscription) { this.notificationServiceSubscription.unsubscribe(); }
  }
}
