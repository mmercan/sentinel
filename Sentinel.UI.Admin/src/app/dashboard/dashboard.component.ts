import { Component, OnInit, OnDestroy, AfterViewInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ImageClassfierService } from '../shared/ai/image-classfier.service';
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
  ngAfterViewInit(): void {
    //  throw new Error("Method not implemented.");
  }
  ngOnDestroy(): void {
    // throw new Error("Method not implemented.");
  }
  async ngOnInit() {
    await this.imageClassfier.loader();
    console.log('ai loadded');
    // throw new Error("Method not implemented.");
  }
}
