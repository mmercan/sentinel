import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StorageComponent } from './storage.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { SharedModule } from '../../shared/shared.module';
import { AppConfig } from '../../app.config';

describe('StorageComponent', () => {
  let component: StorageComponent;
  let fixture: ComponentFixture<StorageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [FormsModule, ReactiveFormsModule],
      declarations: [StorageComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StorageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
