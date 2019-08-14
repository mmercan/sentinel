import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { ImportConfigComponent } from './import-config.component';

describe('ImportConfigComponent', () => {
  let component: ImportConfigComponent;
  let fixture: ComponentFixture<ImportConfigComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [CommonModule, RouterModule.forRoot([]), SharedModule.forRoot()],
      declarations: [ImportConfigComponent],
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImportConfigComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should CofigdataFileChange', () => {

    const fileset = new Array<File>();
    // tslint:disable-next-line:max-line-length
    const content = '{"provider": {"dev": {"healthChecks": [{"isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell","name": "HealthMonitoring Api"}, {"isaliveandwell": "https://comms.api.myrcan.com/health/isaliveandwell","name": "Comms Api"}, {"isaliveandwell": "https://member.api.myrcan.com/health/isaliveandwell","name": "Member Api"}, {"isaliveandwell": "https://product.api.myrcan.com/health/isaliveandwell","name": "Product Api"}, {"isaliveandwell": "https://product.myrcan.com/health/isaliveandwell","name": "Product UI"}, {"isaliveandwell": "https://auth.myrcan.com/health/isaliveandwell","name": "STS UI"}] }, "test": {"healthChecks": [{"isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell","name": "HealthMonitoring Api"}] }, "euat": {"healthChecks": [{"isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell","name": "HealthMonitoring Api"}] }, "perf": {"healthChecks": [{"isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell","name": "HealthMonitoring Api"}] } }, "apollo": { "dev": {"healthChecks": [{"isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell","name": "HealthMonitoring Api"}, {"isaliveandwell": "https://comms.api.myrcan.com/health/isaliveandwell","name": "Comms Api"}, {"isaliveandwell": "https://member.api.myrcan.com/health/isaliveandwell","name": "Member Api"}, {"isaliveandwell": "https://product.api.myrcan.com/health/isaliveandwell","name": "Product Api"}, {"isaliveandwell": "https://product.myrcan.com/health/isaliveandwell","name": "Product UI"}, {"isaliveandwell": "https://auth.myrcan.com/health/isaliveandwell","name": "STS UI"}] } } }';
    const data = new Blob([content], { type: 'text/plain' });
    const arrayOfBlob = new Array<Blob>();
    arrayOfBlob.push(data);
    const file = new File(arrayOfBlob, 'Mock.json');
    fileset.push(file);

    const event = { srcElement: { files: fileset } };
    component.onCofigdataFileChange(event);
    expect(component).toBeTruthy();
  });

  it('should handleError', () => {
    const error = {
      json() {
        return '';
      },

    };
    error['_body'] = 'there is an error';
    const errorMessage = 'here is the error';
    component.handleError(error, errorMessage);
  });

});
