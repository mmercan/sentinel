import { Component, OnInit, ViewChild } from '@angular/core';
import { User } from '../interfaces/user-interface';
// import { } from '../../shared/data-store/user-data-store'

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnInit {
  ProfileImage: string;
  context: any;
  @ViewChild('myCanvas') myCanvas;
  constructor() { }

  ngOnInit() {

    const canvas = this.myCanvas.nativeElement;
    this.context = canvas.getContext('2d');
  }


  newprofilepic(event: any) {
    const files = event.srcElement.files;
    console.log(files);
    if (files[0]) {
      const file = files[0];

      if (file.type === 'image/png' ||
        file.type === 'image/jpeg' ||
        file.type === 'image/gif' ||
        file.type === 'image/bmp' ||
        file.type === 'image/svg+xml' ||
        file.type === 'image/svg'
      ) {

        const reader = new FileReader();
        reader.onloadend = (e: any) => {
          this.ProfileImage = e.target.result;
          const image = new Image();
          image.onload = () => {
            this.context.drawImage(image, 0, 0);
          };
          image.src = e.target.result;
          console.log(e.target.result, e.target.error);
        };
        reader.readAsDataURL(file);


      }


    }
  }
}

