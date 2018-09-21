import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AuthenticationRoutes } from './authentication.routing';
import { SigninComponent } from './signin/signin.component';
import { SignupComponent } from './signup/signup.component';
import { ForgotComponent } from './forgot/forgot.component';
import { LockscreenComponent } from './lockscreen/lockscreen.component';
import { OathCallbackComponent } from './oath-callback/oath-callback.component';
import { OAuthCallbackHandler } from './oauth-callback-guard';
import { SharedModule } from '../shared/shared.module';
import { SigninbComponent } from './signinb/signinb.component';
@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild(AuthenticationRoutes),
    FormsModule,
    ReactiveFormsModule
  ],
  declarations: [SigninComponent, SignupComponent, ForgotComponent, LockscreenComponent, OathCallbackComponent, SigninbComponent],
  providers: [OAuthCallbackHandler]
})

export class AuthenticationModule { }
