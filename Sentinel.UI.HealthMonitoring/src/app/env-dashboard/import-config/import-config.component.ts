import { Component, OnInit } from '@angular/core';
import { ConfigDataService } from '../../shared/data-store/config-data/config-data.service';
import { NotificationService } from '../../shared/notification/notification.service';

@Component({
  selector: 'app-import-config',
  templateUrl: './import-config.component.html',
  styleUrls: ['./import-config.component.scss'],
})
export class ImportConfigComponent implements OnInit {
  configdata: any = {};

  constructor(private configDataService: ConfigDataService, private notificationService: NotificationService) { }

  ngOnInit() {
    this.configDataService.configData.subscribe((data) => {
      this.configdata = data;
    });
  }

  onCofigdataFileChange(event) {
    const files = event.srcElement.files;
    this.configDataService.setConfigData(files[0]).subscribe(
      (data) => { },
      (error) => {
        this.handleError(null, 'File Load Failed');
      });
  }

  handleError(error: any, errorMessage: string) {
    console.error('An error occurred', error);
    if (error && error['_body']) {
      const message = error.json();
    }
    this.notificationService.showError(errorMessage, error as string, true);
  }

}
