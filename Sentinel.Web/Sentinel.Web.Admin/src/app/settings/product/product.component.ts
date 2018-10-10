import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { Subscription } from 'rxjs/internal/Subscription';
import { DragulaService } from 'ng2-dragula';

@Component({
    selector: 'app-product',
    templateUrl: './product.component.html',
    styleUrls: ['./product.component.scss']
})
export class ProductComponent implements OnInit, OnDestroy {
    cachename = 'settings.product';
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
                // this.settings = Object.assign({}, this.settings);
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
                // this.settings = Object.assign({}, this.settings);
                // changeDetectorRef.markForCheck();
            })
        );
    }

    ngOnInit() {
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
        if (!arg) {
            console.log('loading static cache values');
            this.settings = {
                allColumns: [
                    { name: 'Id', prop: 'id', flexGrow: 0, sortable: 'true' },
                    { name: 'Product Code', prop: 'productCode', flexGrow: 0, sortable: 'true' },
                    { name: 'Name', prop: 'name', flexGrow: 0, sortable: 'true' },
                    { name: 'productUrl', prop: 'productUrl', flexGrow: 0, sortable: 'true' },
                    { name: 'active', prop: 'active', flexGrow: 0, sortable: 'true' },
                    { name: 'Html', prop: 'html', flexGrow: 0, sortable: 'true' },
                    { name: 'Description Html', prop: 'descriptionHtml', flexGrow: 0, sortable: 'true' },
                    { name: 'Objectives Html', prop: 'objectivesHtml', flexGrow: 0, sortable: 'true' },
                    { name: 'Audience Html', prop: 'audienceHtml', flexGrow: 0, sortable: 'true' },
                    { name: 'Prerequisites Html', prop: 'prerequisitesHtml', flexGrow: 0, sortable: 'true' },
                    { name: 'Topics Html', prop: 'topicsHtml', flexGrow: 0, sortable: 'true' },
                    { name: 'Related Html', prop: 'relatedHtml', flexGrow: 0, sortable: 'true' },
                    { name: 'Roadmaps Html', prop: 'roadmapsHtml', flexGrow: 0, sortable: 'true' },
                    { name: 'Duration', prop: 'duration', flexGrow: 0, sortable: 'true' },
                    { name: 'Duration Type', prop: 'durationType', flexGrow: 0, sortable: 'true' },
                    { name: 'Created On', prop: 'createdOn', flexGrow: 0, sortable: 'true' },
                    { name: 'Modified On', prop: 'modifiedOn', flexGrow: 0, sortable: 'true' },
                    { name: 'Technology Id', prop: 'technologyId', flexGrow: 0, sortable: 'true' },
                    { name: 'Technology Name', prop: 'technologyName', flexGrow: 0, sortable: 'true' },
                    { name: 'Technology Url', prop: 'technologyUrl', flexGrow: 0, sortable: 'true' },
                    { name: 'Vendor Id', prop: 'vendorId', flexGrow: 0, sortable: 'true' },
                    { name: 'Vendor Name', prop: 'vendorName', flexGrow: 0, sortable: 'true' },
                    { name: 'Vendor Url', prop: 'vendorUrl', flexGrow: 0, sortable: 'true' }
                ],
                selectedColumns: [{ name: 'Id', prop: 'id', flexGrow: 1, sortable: 'true' },
                { name: 'Product Code', prop: 'productCode', flexGrow: 1, sortable: 'true' },
                { name: 'Name', prop: 'name', flexGrow: 2, sortable: 'true' },
                { name: 'Active', prop: 'active', flexGrow: 1, sortable: 'true' }],
                availableColumns: [],
                filters: {}
            };

            const diff = this.arrDiff(this.settings.allColumns, this.settings.selectedColumns);
            this.settings.allColumns.forEach(ob => {
                if (diff.includes(ob.prop)) {
                    this.settings.availableColumns.push(ob);
                }
            });
            console.log('diff');
            console.log(diff);
        }
    }

    public save() {
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
