import { Component, OnInit, OnDestroy, AfterViewInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ImageClassfierService } from '../shared/ai/image-classfier.service';
import { DrawableDirective } from '../shared/drawable/drawable.directive';
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})

export class DashboardComponent implements OnInit, OnDestroy, AfterViewInit {

  constructor(private httpClient: HttpClient, private imageClassfier: ImageClassfierService) {
    httpClient.get('/blah123.json').subscribe(data => {

    },
      error => {

      }
    );
  }


  @ViewChild(DrawableDirective) canvas;

  predictions: any;
  predictionsobject: any[] = [];
  ngAfterViewInit(): void {
    //  throw new Error("Method not implemented.");
  }
  ngOnDestroy(): void {
    // throw new Error("Method not implemented.");
  }
  async ngOnInit() {


    await this.imageClassfier.trainNewModel();
    await this.imageClassfier.loadModel();

    // await this.imageClassfier.train();
    // console.log('ai loadded');
    // this.imageClassfier.predict(20);
    // throw new Error("Method not implemented.");




  }
  async predict(imageData: ImageData) {
    await this.imageClassfier.predict(imageData);
    this.predictions = this.imageClassfier.predictions;
    this.predictionsobject = [];
    let i = 0;
    this.predictions.forEach(e => {
      this.predictionsobject.push({ item: i, value: e.toFixed(15) });
      i++;
    });


  }
}
