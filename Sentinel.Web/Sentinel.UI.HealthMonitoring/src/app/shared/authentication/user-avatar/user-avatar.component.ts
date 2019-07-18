import { AfterViewInit, Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AppConfig, authenticationType } from '../../../app.config';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-user-avatar',
  templateUrl: './user-avatar.component.html',
  styleUrls: ['./user-avatar.component.scss'],
})
export class UserAvatarComponent implements OnInit, AfterViewInit, OnDestroy {
  container: any;
  getUserInfoSubscription: Subscription;
  constructor(private authService: AuthService, private appConfig: AppConfig) { }

  @Input()
  height: number;

  ngOnInit() {

  }

  ngAfterViewInit() {
    this.container = document.getElementById('canvas-container');
    if (this.authService.authenticated) {
      this.getUserInfoSubscription = this.authService.getUserInfo().subscribe((data) => {
        console.log(JSON.stringify(data));

        if (data.profile && data.profile.name) {
          this.createLetterAvatar(data.profile.name, this.container, this.height);
        }
      });

    } else {
      console.log('is Not Authenticated');
    }
  }

  createLetterAvatar(name: string, htmlElement: HTMLElement, height) {
    const colours = ['#1abc9c', '#2ecc71', '#3498db', '#9b59b6', '#34495e', '#16a085', '#27ae60', '#2980b9', '#8e44ad',
      '#2c3e50', '#f1c40f', '#e67e22', '#e74c3c', '#95a5a6', '#f39c12', '#d35400', '#c0392b', '#bdc3c7', '#7f8c8d'];
    const nameSplit = name.split(' ');
    const initials = nameSplit[0].charAt(0).toUpperCase() + nameSplit[1].charAt(0).toUpperCase();
    const charIndex = initials.charCodeAt(0) - 65;
    const colourIndex = Math.floor(Math.random() * 19);
    let canvas: HTMLCanvasElement;
    const cw = height ? height : 32;
    const ch = cw;
    canvas = document.createElement('canvas');
    const ctx = canvas.getContext('2d');
    canvas.width = cw;
    canvas.height = ch;
    canvas.style.verticalAlign = 'middle';
    ctx.fillStyle = colours[colourIndex];
    ctx.fillRect(0, 0, canvas.width, canvas.height);
    const fontsize = Math.floor(cw / 2);
    ctx.font = fontsize + 'px Arial';
    ctx.textAlign = 'center';
    ctx.fillStyle = '#FFF';
    ctx.fillText(initials, cw / 2, ch / 1.5);

    htmlElement.appendChild(canvas);
  }
  ngOnDestroy(): void {
    if (this.getUserInfoSubscription) { this.getUserInfoSubscription.unsubscribe(); }
  }
}
