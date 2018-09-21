import { Component, OnInit } from '@angular/core';
import { ProductDataStoreService } from '../../shared/data-store/product-data-store/product-data-store.service';
// import { Product } from '../../shared/data-store/product-data-store/Interfaces/Production';
@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit {

  constructor(private productDS: ProductDataStoreService) { }

  ngOnInit() {
    this.productDS.dataset.subscribe(res => {
      console.log(res);
    });
    this.productDS.getAll();

  }

}
