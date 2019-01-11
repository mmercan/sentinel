import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { Subscription } from 'rxjs/internal/Subscription';
import { DragulaService } from 'ng2-dragula';
import { ActivatedRoute } from '@angular/router';
import { filter } from 'rxjs/operators';
import { SettingsService } from './settings.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnInit, OnDestroy {

  cachename = 'settings.product';
  MANY_ITEMS = 'MANY_ITEMS';
  public settings: any;
  subs = new Subscription();
  constructor(private dragulaService: DragulaService, private changeDetectorRef: ChangeDetectorRef, private route: ActivatedRoute,
    private settingsService: SettingsService
  ) {

    this.subs.add(dragulaService.dropModel(this.MANY_ITEMS)
      .subscribe(({ el, target, source, sourceModel, targetModel, item }) => {
        console.log('dropModel:');
        // console.log(el); console.log(source); console.log(target); console.log(sourceModel); console.log(targetModel);
        console.log(item);
        // this.settings = Object.assign({}, this.settings); changeDetectorRef.markForCheck();
      })
    );

    this.subs.add(dragulaService.removeModel(this.MANY_ITEMS)
      .subscribe(({ el, source, item, sourceModel }) => {
        console.log('removeModel:');
        // console.log(el); console.log(source); console.log(sourceModel);
        console.log(item);
        // this.settings = Object.assign({}, this.settings); changeDetectorRef.markForCheck();
      })
    );
  }

  ngOnInit() {

    this.route.queryParams.pipe(filter(params => params.entity))
      .subscribe(params => {
        console.log(params);
        const cachename = this.settingsService.getCacheName(params.entity);
        this.cachename = cachename;
      });
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  onCacheChange(data) {
    console.log('onCacheChange');
    // console.log(data);
    this.loadsettings(data);
  }

  loadsettings(arg: any) {
    console.log('loadsettings');
    console.log(arg);
    const diff = this.arrDiff(this.settings.allColumns, this.settings.selectedColumns);
    this.settings.allColumns.forEach(ob => {
      if (diff.includes(ob.prop)) {
        this.settings.availableColumns.push(ob);
      }
    });
    console.log('diff');
    console.log(diff);
  }

  public save() {
    this.settings.availableColumn = [];
    const stringSettings = JSON.stringify(this.settings);
    localStorage.setItem(this.cachename, stringSettings);
  }

  public addLink(item) {
    if (item.link) {
      item.cellTemplate = '';
    } else {
      item.cellTemplate = undefined;
    }
  }

  private arrDiff(a1: any[], a2: any[]) {
    const a = [], diff = [];

    for (let i = 0; i < a1.length; i++) {
      a[a1[i].prop] = true;
    }

    for (let i = 0; i < a2.length; i++) {
      if (a[a2[i].prop]) {
        delete a[a2[i].prop];
      } else {
        a[a2[i].prop] = true;
      }
    }

    for (const k in a) {
      diff.push(k);
    }
    return diff;
  }

}
