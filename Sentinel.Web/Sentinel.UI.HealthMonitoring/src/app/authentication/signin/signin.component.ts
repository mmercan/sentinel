import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { AppConfig, authenticationType, logLevel } from '../../app.config';
import { AuthService } from '../../shared/authentication/auth.service';
@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.scss']
})
export class SigninComponent implements OnInit {

  public form: FormGroup;
  constructor(private fb: FormBuilder, private router: Router, private appConfig: AppConfig, private authService: AuthService) { }

  ngOnInit() {
    if (this.appConfig.config.authenticationType === authenticationType.Adal) {
      this.authService.login();
    }
    this.form = this.fb.group({
      uname: [null, Validators.compose([Validators.required])], password: [null, Validators.compose([Validators.required])]
    });
  }

  onSubmit() {
    this.router.navigate(['/']);
    this.authService.login(this.form.get('uname').value, this.form.get('password').value);
  }

}
