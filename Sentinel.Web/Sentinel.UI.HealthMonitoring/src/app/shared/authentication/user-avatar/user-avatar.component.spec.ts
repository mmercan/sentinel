import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { AppConfig, authenticationType, logLevel } from '../../../app.config';
import { AdalService } from '../../authentication/adal-auth/adal.service';
import { NotificationService } from '../../notification/notification.service';
import { AuthService } from '../auth.service';
import { UserAvatarComponent } from './user-avatar.component';
import { Subscription, Observable } from 'rxjs';

export class MockAuthService {

  public authenticated = true;

  data = {
    profile: { name: 'Matt Mercan' },
  };

  getUserInfo(): Observable<any> {
    const obs = Observable.create((observer) => {
      observer.next(this.data);
      //  error => { observer.error(error); });
    });
    return obs;
  }
}

describe('UserAvatarComponent', () => {
  let component: UserAvatarComponent;
  let fixture: ComponentFixture<UserAvatarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [CommonModule, HttpClientModule, RouterModule.forRoot([])],
      declarations: [UserAvatarComponent],
      providers: [AppConfig, NotificationService, { provide: AdalService, useClass: MockAuthService },
        { provide: AuthService, useClass: MockAuthService }],
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserAvatarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should create avatar', () => {
    const canvas = document.createElement('canvas');
    // canvas: HTMLCanvasElement = document.getElementById('canvas') as HTMLCanvasElement;
    expect(component.createLetterAvatar('Matt Mercan', canvas, 32)).toBeUndefined();
  });

  it('should create avatar', () => {
    expect(component.ngAfterViewInit()).toBeUndefined();
  });

  it('should warn if not authenticated', () => {
    const userService = fixture.debugElement.injector.get(AuthService);
    // const saveSpy = spyOn(userService, 'authenticated').and.callThrough();

    userService.authenticated = undefined;

    expect(component.ngAfterViewInit()).toBeUndefined();
  });

});
