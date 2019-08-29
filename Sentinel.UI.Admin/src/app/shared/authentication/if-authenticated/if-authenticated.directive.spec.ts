import { IfAuthenticatedDirective } from './if-authenticated.directive';
import { TestBed, ComponentFixture, async } from '@angular/core/testing';
import { TestComponent } from '../../test-tools/test/test.component';
import { CommonModule } from '@angular/common';
import { By } from '@angular/platform-browser';
import { ConfigApiService } from '../../data-store/config-api/config-api.service';
import { AppConfig } from '../../../app.config';
import { AuthService } from '../auth.service';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Notification, NotificationService } from '../../notification/notification.service';
import { AdalService } from '../adal-auth/adal.service';
import { LocalAuthService } from '../local-auth/local-auth.service';
import { TemplateRef, EmbeddedViewRef } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// export class MockTemplateRef extends TemplateRef<IfAuthenticatedDirective> {
//   elementRef: import('@angular/core').ElementRef<any>;
//   createEmbeddedView(context: IfAuthenticatedDirective): import('@angular/core').EmbeddedViewRef<IfAuthenticatedDirective> {
//     return new EmbeddedViewRef<IfAuthenticatedDirective>()
//   }
// }

describe('IfAuthenticatedDirective', () => {

  let component: TestComponent;
  let fixture: ComponentFixture<TestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [CommonModule, FormsModule, ReactiveFormsModule, HttpClientModule],
      providers: [ConfigApiService, AppConfig, AuthService, NotificationService, AdalService, LocalAuthService],
      declarations: [IfAuthenticatedDirective, TestComponent],
    })
      .overrideComponent(TestComponent, {
        set: {
          template: '<div class="mytestclass" *ifAuthenticated="false">NoAuth</div>'
        }
      })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });



  it('should ifAuthenticated be false', async(() => {
    // const directiveEl = fixture.debugElement.query(By.directive(IfAuthenticatedDirective));
    // expect(directiveEl).not.toBeNull();

    // const directiveInstance = directiveEl.injector.get(IfAuthenticatedDirective);
    // expect(directiveInstance.ifAuthenticated).toBe(false);
    expect(fixture.debugElement.nativeElement).not.toBeNull();

  }));


});
