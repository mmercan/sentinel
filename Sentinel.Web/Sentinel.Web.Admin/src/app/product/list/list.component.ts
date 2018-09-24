import { Component, OnInit } from '@angular/core';
import { ProductDataStoreService } from '../../shared/data-store/product-data-store/product-data-store.service';
// import { Product } from '../../shared/data-store/product-data-store/Interfaces/Production';
@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit {
  pageView = 'table';
  constructor(private productDS: ProductDataStoreService) { }

  rows = [];
  loadingIndicator = true;
  reorderable = true;

  columns = [
    { prop: 'name' },
    { name: 'Gender' },
    { name: 'Company', sortable: false }
  ];


  ngOnInit() {
    this.productDS.dataset.subscribe(res => {
      console.log(res);
      this.loadingIndicator = false;
    });
    this.productDS.getAll();

  }

}
