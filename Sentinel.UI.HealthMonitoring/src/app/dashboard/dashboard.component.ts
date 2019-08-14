import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})

export class DashboardComponent {

  constructor(private httpClient: HttpClient) {
    // httpClient.get('/blah.json').subscribe(data => { },     error => { });
  }
}
