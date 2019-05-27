import { Directive, OnInit, AfterViewInit, AfterContentChecked, OnDestroy } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { AccordionLinkDirective } from './accordionlink.directive';
import { filter } from 'rxjs/operators';
import { Subscription } from 'rxjs';

@Directive({
  selector: '[appAccordion]',
})
export class AccordionDirective implements AfterContentChecked, OnDestroy {
  protected navlinks: Array<AccordionLinkDirective> = [];
  routerSubscription: Subscription;

  closeOtherLinks(openLink: AccordionLinkDirective): void {
    this.navlinks.forEach((link: AccordionLinkDirective) => {
      if (link !== openLink) {
        link.open = false;
      }
    });
  }

  addLink(link: AccordionLinkDirective): void {
    this.navlinks.push(link);
  }

  removeGroup(link: AccordionLinkDirective): void {
    const index = this.navlinks.indexOf(link);
    if (index !== -1) {
      this.navlinks.splice(index, 1);
    }
  }

  checkOpenLinks() {
    this.navlinks.forEach((link: AccordionLinkDirective) => {
      if (link.group) {
        const routeUrl = this.router.url;
        const currentUrl = routeUrl.split('/');
        if (currentUrl.indexOf(link.group) > 0) {
          link.open = true;
          this.closeOtherLinks(link);
        }
      }
    });
  }

  ngAfterContentChecked(): void {
    this.routerSubscription = this.router.events.pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(e => this.checkOpenLinks());
  }

  constructor(private router: Router) {
    setTimeout(() => this.checkOpenLinks());
  }

  ngOnDestroy(): void {
    this.routerSubscription.unsubscribe();
  }
}
