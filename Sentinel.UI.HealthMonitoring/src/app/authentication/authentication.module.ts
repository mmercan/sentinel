import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { AuthenticationRoutes } from './authentication.routing';
import { ForgotComponent } from './forgot/forgot.component';
import { SigninComponent } from './signin/signin.component';
import { SignupComponent } from './signup/signup.component';

import { LockscreenComponent } from './lockscreen/lockscreen.component';
import { OathCallbackComponent } from './oath-callback/oath-callback.component';
import { OAuthCallbackHandler } from './oauth-callback-guard';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild(AuthenticationRoutes),
    FormsModule,
    ReactiveFormsModule,
  ],
  declarations: [SigninComponent, SignupComponent, ForgotComponent, LockscreenComponent, OathCallbackComponent],
  providers: [OAuthCallbackHandler]
})

export class AuthenticationModule { }
