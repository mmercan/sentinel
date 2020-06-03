import { Injectable } from '@angular/core';
import * as mobilenet from '@tensorflow-models/mobilenet';

@Injectable({
  providedIn: 'root'
})
export class ImageClassfierService {
  loading: boolean;
  model: mobilenet.MobileNet;

  constructor() {


  }

  async loader() {
    this.loading = true;
    this.model = await mobilenet.load();
    this.loading = false;

  }

}
