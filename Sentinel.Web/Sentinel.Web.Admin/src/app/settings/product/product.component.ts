import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { Subscription } from 'rxjs/internal/Subscription';
import { DragulaService } from 'ng2-dragula';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss']
})
export class ProductComponent implements OnInit, OnDestroy {

  MANY_ITEMS = 'MANY_ITEMS';
  public settings: any;


  subs = new Subscription();

  constructor(private dragulaService: DragulaService, private changeDetectorRef: ChangeDetectorRef) {

    this.subs.add(dragulaService.dropModel(this.MANY_ITEMS)
      .subscribe(({ el, target, source, sourceModel, targetModel, item }) => {
        console.log('dropModel:');
        // console.log(el);
        // console.log(source);
        // console.log(target);
        // console.log(sourceModel);
        // console.log(targetModel);
        console.log(item);
        this.settings = Object.assign({}, this.settings);
        // changeDetectorRef.markForCheck();
      })
    );

    this.subs.add(dragulaService.removeModel(this.MANY_ITEMS)
      .subscribe(({ el, source, item, sourceModel }) => {
        console.log('removeModel:');
        // console.log(el);
        // console.log(source);
        // console.log(sourceModel);
        console.log(item);
        this.settings = Object.assign({}, this.settings);
        // changeDetectorRef.markForCheck();
      })
    );

  }
  sliderValue = 500;

  ngOnInit() {
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  onCacheChange(arg: any) {
    console.log('onCacheChange');
    console.log(arg);
    if (!arg) {
      console.log('loading static cache values');
      this.settings = {
        allColumns: [
          { name: 'id', prop: 'id', flexGrow: 0, sortable: 'true' },
          { name: 'productCode', prop: 'productCode', flexGrow: 0, sortable: 'true' },
          { name: 'name', prop: 'name', flexGrow: 0, sortable: 'true' },
          { name: 'productUrl', prop: 'productUrl', flexGrow: 0, sortable: 'true' },
          { name: 'active', prop: 'active', flexGrow: 0, sortable: 'true' },
          { name: 'html', prop: 'html', flexGrow: 0, sortable: 'true' },
          { name: 'descriptionHtml', prop: 'descriptionHtml', flexGrow: 0, sortable: 'true' },
          { name: 'objectivesHtml', prop: 'objectivesHtml', flexGrow: 0, sortable: 'true' },
          { name: 'audienceHtm', prop: 'audienceHtm', flexGrow: 0, sortable: 'true' },
          { name: 'prerequisitesHtml', prop: 'prerequisitesHtml', flexGrow: 0, sortable: 'true' },
          { name: 'topicsHtml', prop: 'topicsHtml', flexGrow: 0, sortable: 'true' },
          { name: 'relatedHtml', prop: 'relatedHtml', flexGrow: 0, sortable: 'true' },
          { name: 'roadmapsHtml', prop: 'roadmapsHtml', flexGrow: 0, sortable: 'true' },
          { name: 'duration', prop: 'duration', flexGrow: 0, sortable: 'true' },
          { name: 'durationType', prop: 'durationType', flexGrow: 0, sortable: 'true' },
          { name: 'createdOn', prop: 'createdOn', flexGrow: 0, sortable: 'true' },
          { name: 'modifiedOn', prop: 'modifiedOn', flexGrow: 0, sortable: 'true' },
          { name: 'technologyId', prop: 'technologyId', flexGrow: 0, sortable: 'true' },
          { name: 'technologyName', prop: 'technologyName', flexGrow: 0, sortable: 'true' },
          { name: 'technologyUrl', prop: 'technologyUrl', flexGrow: 0, sortable: 'true' },
          { name: 'vendorId', prop: 'vendorId', flexGrow: 0, sortable: 'true' },
          { name: 'vendorName', prop: 'vendorName', flexGrow: 0, sortable: 'true' },
          { name: 'vendorUrl', prop: 'vendorUrl', flexGrow: 0, sortable: 'true' }
        ],
        selectedColumns: [{ name: 'id', prop: 'id', flexGrow: 0, sortable: 'true' },
        { name: 'productCode', prop: 'productCode', flexGrow: 0, sortable: 'true' },
        { name: 'name', prop: 'name', flexGrow: 0, sortable: 'true' },
        { name: 'productUrl', prop: 'productUrl', flexGrow: 0, sortable: 'true' },
        { name: 'active', prop: 'active', flexGrow: 0, sortable: 'true' },],
        availableColumns: [{ name: 'The' }, { name: 'possibilities' }, { name: 'are' }, { name: 'endless!' }]
      };

      const diff = this.arrDiff(this.settings.allColumns, this.settings.selectedColumns);
      console.log('diff');
      console.log(diff);
    }
  }

  arrDiff(a1: any[], a2: any[]) {
    const a = [], diff = [];

    for (let i = 0; i < a1.length; i++) {
      a[a1[i].name] = true;
    }

    for (let i = 0; i < a2.length; i++) {
      if (a[a2[i].name]) {
        delete a[a2[i]];
      } else {
        a[a2[i].name] = true;
      }
    }

    for (const k in a) {
      diff.push(k);
    }

    return diff;
  }

}
