import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { CommonDataStoreService } from '../common-data-store/common-data-store.service';
import { Product } from './Interfaces/Production';
import { AppConfig } from '../../../app.config';
import { CommonDataStoreInterface } from '../common-data-store/common-data-store-interface';

@Injectable()
export class ProductDataStoreService extends CommonDataStoreService<Product> {



  constructor(http: Http, appConfig: AppConfig) {
    const productApiUrl = `${appConfig.config.Api.baseUrl}Product`;
    // this.dataservice = new CommonDataStoreService<Product>(http, productApiUrl, 'ProductId');

    super(http, productApiUrl, 'ProductId');
  }

}
