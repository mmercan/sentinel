import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { ConfigDataService } from '../shared/data-store/config-data/config-data.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})

export class DashboardComponent {

  constructor(private httpClient: HttpClient, private configDataService: ConfigDataService) {

    configDataService.getConfigData();
    // httpClient.get('/blah.json').subscribe(data => { },     error => { });
  }
}
