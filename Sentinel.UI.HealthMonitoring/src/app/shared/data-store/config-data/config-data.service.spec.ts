import { TestBed } from '@angular/core/testing';

import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { ConfigDataService } from './config-data.service';

class MockHTMLInput {
  files: File[];
  constructor() {
    this.files = new Array<File>();
    // tslint:disable-next-line:max-line-length
    const content = '{"provider": {"dev": {"healthChecks": [{"isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell","name": "HealthMonitoring Api"}, {"isaliveandwell": "https://comms.api.myrcan.com/health/isaliveandwell","name": "Comms Api"}, {"isaliveandwell": "https://member.api.myrcan.com/health/isaliveandwell","name": "Member Api"}, {"isaliveandwell": "https://product.api.myrcan.com/health/isaliveandwell","name": "Product Api"}, {"isaliveandwell": "https://product.myrcan.com/health/isaliveandwell","name": "Product UI"}, {"isaliveandwell": "https://auth.myrcan.com/health/isaliveandwell","name": "STS UI"}] }, "test": {"healthChecks": [{"isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell","name": "HealthMonitoring Api"}] }, "euat": {"healthChecks": [{"isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell","name": "HealthMonitoring Api"}] }, "perf": {"healthChecks": [{"isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell","name": "HealthMonitoring Api"}] } }, "apollo": { "dev": {"healthChecks": [{"isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell","name": "HealthMonitoring Api"}, {"isaliveandwell": "https://comms.api.myrcan.com/health/isaliveandwell","name": "Comms Api"}, {"isaliveandwell": "https://member.api.myrcan.com/health/isaliveandwell","name": "Member Api"}, {"isaliveandwell": "https://product.api.myrcan.com/health/isaliveandwell","name": "Product Api"}, {"isaliveandwell": "https://product.myrcan.com/health/isaliveandwell","name": "Product UI"}, {"isaliveandwell": "https://auth.myrcan.com/health/isaliveandwell","name": "STS UI"}] } } }';
    const data = new Blob([content], { type: 'text/plain' });
    const arrayOfBlob = new Array<Blob>();
    arrayOfBlob.push(data);
    const file = new File(arrayOfBlob, 'Mock.json');
    this.files.push(file);
  }
}

describe('ConfigDataServiceService', () => {

  beforeEach(() => TestBed.configureTestingModule({
    imports: [CommonModule, HttpClientModule],
    // providers: [{ provide: HTMLInputElement, useClass: MockHTMLInput }]
  }));

  it('should be created', () => {
    const service: ConfigDataService = TestBed.get(ConfigDataService);
    expect(service).toBeTruthy();
  });

  it('should be getConfigData', () => {
    const service: ConfigDataService = TestBed.get(ConfigDataService);
    // tslint:disable-next-line:max-line-length
    const data = '{ "provider": { "dev": {"healthChecks": [{"isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell","name": "HealthMonitoring Api"}, {"isaliveandwell": "https://comms.api.myrcan.com/health/isaliveandwell","name": "Comms Api"}, {"isaliveandwell": "https://member.api.myrcan.com/health/isaliveandwell","name": "Member Api"}, {"isaliveandwell": "https://product.api.myrcan.com/health/isaliveandwell","name": "Product Api"}, {"isaliveandwell": "https://product.myrcan.com/health/isaliveandwell","name": "Product UI"}, {"isaliveandwell": "https://auth.myrcan.com/health/isaliveandwell","name": "STS UI"}] }, "test": {"healthChecks": [{"isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell","name": "HealthMonitoring Api"}] }, "euat": {"healthChecks": [{"isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell","name": "HealthMonitoring Api"}] }, "perf": {"healthChecks": [{"isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell","name": "HealthMonitoring Api"}] } }, "apollo": { "dev": {"healthChecks": [{"isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell","name": "HealthMonitoring Api"}, {"isaliveandwell": "https://comms.api.myrcan.com/health/isaliveandwell","name": "Comms Api"}, {"isaliveandwell": "https://member.api.myrcan.com/health/isaliveandwell","name": "Member Api"}, {"isaliveandwell": "https://product.api.myrcan.com/health/isaliveandwell","name": "Product Api"}, {"isaliveandwell": "https://product.myrcan.com/health/isaliveandwell","name": "Product UI"}, {"isaliveandwell": "https://auth.myrcan.com/health/isaliveandwell","name": "STS UI"}] } } }';
    localStorage.setItem('app-configData', data);
    service.getConfigData();
  });

  it('should be setConfigData', () => {
    const service: ConfigDataService = TestBed.get(ConfigDataService);

    // tslint:disable-next-line:max-line-length
    const content = '{"provider": {"dev": {"healthChecks": [{"isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell","name": "HealthMonitoring Api"}, {"isaliveandwell": "https://comms.api.myrcan.com/health/isaliveandwell","name": "Comms Api"}, {"isaliveandwell": "https://member.api.myrcan.com/health/isaliveandwell","name": "Member Api"}, {"isaliveandwell": "https://product.api.myrcan.com/health/isaliveandwell","name": "Product Api"}, {"isaliveandwell": "https://product.myrcan.com/health/isaliveandwell","name": "Product UI"}, {"isaliveandwell": "https://auth.myrcan.com/health/isaliveandwell","name": "STS UI"}] }, "test": {"healthChecks": [{"isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell","name": "HealthMonitoring Api"}] }, "euat": {"healthChecks": [{"isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell","name": "HealthMonitoring Api"}] }, "perf": {"healthChecks": [{"isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell","name": "HealthMonitoring Api"}] } }, "apollo": { "dev": {"healthChecks": [{"isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell","name": "HealthMonitoring Api"}, {"isaliveandwell": "https://comms.api.myrcan.com/health/isaliveandwell","name": "Comms Api"}, {"isaliveandwell": "https://member.api.myrcan.com/health/isaliveandwell","name": "Member Api"}, {"isaliveandwell": "https://product.api.myrcan.com/health/isaliveandwell","name": "Product Api"}, {"isaliveandwell": "https://product.myrcan.com/health/isaliveandwell","name": "Product UI"}, {"isaliveandwell": "https://auth.myrcan.com/health/isaliveandwell","name": "STS UI"}] } } }';
    const data = new Blob([content], { type: 'text/plain' });
    const arrayOfBlob = new Array<Blob>();
    arrayOfBlob.push(data);
    const file = new File(arrayOfBlob, 'Mock.json');

    service.setConfigData(file);
  });



  it('should be getHealthCheckUrls', () => {
    const service: ConfigDataService = TestBed.get(ConfigDataService);

    // tslint:disable-next-line:max-line-length
    const content = '{"provider": {"dev": {"healthChecks": [{"isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell","name": "HealthMonitoring Api"}, {"isaliveandwell": "https://comms.api.myrcan.com/health/isaliveandwell","name": "Comms Api"}, {"isaliveandwell": "https://member.api.myrcan.com/health/isaliveandwell","name": "Member Api"}, {"isaliveandwell": "https://product.api.myrcan.com/health/isaliveandwell","name": "Product Api"}, {"isaliveandwell": "https://product.myrcan.com/health/isaliveandwell","name": "Product UI"}, {"isaliveandwell": "https://auth.myrcan.com/health/isaliveandwell","name": "STS UI"}] }, "test": {"healthChecks": [{"isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell","name": "HealthMonitoring Api"}] }, "euat": {"healthChecks": [{"isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell","name": "HealthMonitoring Api"}] }, "perf": {"healthChecks": [{"isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell","name": "HealthMonitoring Api"}] } }, "apollo": { "dev": {"healthChecks": [{"isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell","name": "HealthMonitoring Api"}, {"isaliveandwell": "https://comms.api.myrcan.com/health/isaliveandwell","name": "Comms Api"}, {"isaliveandwell": "https://member.api.myrcan.com/health/isaliveandwell","name": "Member Api"}, {"isaliveandwell": "https://product.api.myrcan.com/health/isaliveandwell","name": "Product Api"}, {"isaliveandwell": "https://product.myrcan.com/health/isaliveandwell","name": "Product UI"}, {"isaliveandwell": "https://auth.myrcan.com/health/isaliveandwell","name": "STS UI"}] } } }';
    localStorage.setItem('app-configData', content);

    service.getHealthCheckUrls('provider', 'dev');
  });



});
