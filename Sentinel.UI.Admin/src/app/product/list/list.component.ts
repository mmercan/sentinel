import { Component, OnInit, InjectionToken, Injector } from '@angular/core';
import { ProductDataStoreService } from '../../shared/data-store/product-data-store/product-data-store.service';
import { Router } from '@angular/router';
// import { Product } from '../../shared/data-store/product-data-store/Interfaces/Production';
@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit {
  pageView = 'table';
  public settings: any;
  public filters: any = {};
  public selectedItem: any;
  public listtemplate = '<span> {{item.productCode}} Hello <input [(ngModel)]=\"item.productCode\"> </span>';
  constructor(private productDS: ProductDataStoreService, private router: Router) { }

  rows = [];
  loadingIndicator = true;
  reorderable = true;

  columns = [
    { prop: 'name' },
    { name: 'Gender' },
    { name: 'Company', sortable: false }
  ];


  ngOnInit() {

    // const BASE_URL = new InjectionToken<string>('API_BASE_URL');
    // const injector = Injector.create({ providers: [{ provide: BASE_URL, useValue: 'http://localhost:5003' }] });

    this.productDS.dataset.subscribe(res => {
      console.log(res);
      this.loadingIndicator = false;
      this.rows = res;
    });
    this.productDS.getAll();

  }

  // ngDoCheck() {
  //   // const change = this.differ.diff(this.value);
  //   console.log('ngDoCheck');
  //   console.log(this.settings);
  // }

  newproduct() {
    console.log(this.settings);
  }

  // opensettings() {
  //   this.router.navigate(['/settings/'], { queryParams: { entity: 'product' } });
  // }


  // onCacheChange(settings) {
  //   console.log('onCacheChange');
  //   console.log(settings);
  // }
}
