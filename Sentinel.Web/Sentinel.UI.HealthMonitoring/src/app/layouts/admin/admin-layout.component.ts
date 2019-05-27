import { Component, OnInit, OnDestroy, ViewChild, HostListener, NgZone, AfterViewInit } from '@angular/core';

import { Title } from '@angular/platform-browser';
import { Router, ActivatedRoute, NavigationEnd, NavigationStart } from '@angular/router';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';

import { MenuItems } from '../../shared/menu-items/menu-items';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';


import { OfflineNotificationService } from '../../shared/offline/offline-notification.service';
import { NotificationService, Notification } from '../../shared/notification/notification.service';
import { AuthService } from '../../shared/authentication/auth.service';
import { AppConfig, authenticationType } from '../../app.config';

const SMALL_WIDTH_BREAKPOINT = 991;

export interface Options {
  heading?: string;
  removeFooter?: boolean;
  mapHeader?: boolean;
}

@Component({
  selector: 'app-layout',
  templateUrl: './admin-layout.component.html',
  styleUrls: ['./admin-layout.component.scss']
})
export class AdminLayoutComponent implements OnInit, OnDestroy, AfterViewInit {

  private _router: Subscription;
  private mediaMatcher: MediaQueryList = matchMedia(`(max-width: ${SMALL_WIDTH_BREAKPOINT}px)`);

  currentLang = 'en';
  options: Options;
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
  @ViewChild('sidebar') sidebar;
  @ViewChild('notificationDrop') notificationDrop;
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
  ) {
    this.mediaMatcher.addListener(mql => zone.run(() => this.mediaMatcher = mql));

    this.notificationServiceSubscription = this.notificationService.dataset.subscribe(result => {
      if (result && result.length) {
        this.notificationCount = result.length;
        if (this.notificationDrop) {
          setTimeout(() => { this.notificationDrop.open(); }, 500);
        }
      } else {
        this.notificationCount = 0;
      }
    });
  }

  ngOnInit(): void {

    if (this.isOver()) {
      this._mode = 'over';
      this.isOpened = false;
    }

    this._router = this.router.events.pipe(filter(event => event instanceof NavigationEnd)).subscribe((event: NavigationEnd) => {
      // Scroll to top on view load
      document.querySelector('.main-content').scrollTop = 0;
      this.runOnRouteChange();
      this.appConfig.navigatingto = null;
    });
    this.runOnRouteChange();
  }

  ngAfterViewInit(): void {
    setTimeout(_ => this.runOnRouteChange());
  }

  ngOnDestroy() {
    this._router.unsubscribe();
    this.notificationServiceSubscription.unsubscribe();
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
        { state: 'menu', name: 'MENU' }
      ]
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
}
