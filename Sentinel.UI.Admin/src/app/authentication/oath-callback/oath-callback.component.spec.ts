import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { OathCallbackComponent } from './oath-callback.component';
import { RouterModule } from '@angular/router';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppConfig } from '../../app.config';
import { SharedModule } from '../../shared/shared.module';

describe('OathCallbackComponent', () => {
  let component: OathCallbackComponent;
  let fixture: ComponentFixture<OathCallbackComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [RouterModule.forRoot([]), SharedModule.forRoot(), HttpClientModule],
      declarations: [OathCallbackComponent],
      providers: [AppConfig]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OathCallbackComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
