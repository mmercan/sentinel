import { Component, OnInit, AfterViewInit, Input } from '@angular/core';
import { AuthService } from '../auth.service';
import { AppConfig, authenticationType } from '../../../app.config';
import { Jsonp } from '@angular/http';

@Component({
  selector: 'app-user-avatar',
  templateUrl: './user-avatar.component.html',
  styleUrls: ['./user-avatar.component.scss']
})
export class UserAvatarComponent implements OnInit, AfterViewInit {

  // canvas: HTMLCanvasElement;
  // ctx: CanvasRenderingContext2D;
  container: any;
  // cw: any;
  // ch: any;

  constructor(private authService: AuthService, private appConfig: AppConfig) { }

  @Input()
  height: number;

  ngOnInit() {


  }

  ngAfterViewInit() {
    this.container = document.getElementById('canvas-container');
    // this.cw = this.container.clientWidth;
    // this.ch = this.container.clientHeight;
    // this.canvas = document.createElement('canvas');
    // this.ctx = this.canvas.getContext('2d');
    // this.canvas.width = this.cw;
    // this.canvas.height = this.ch;
    // this.container.appendChild(this.canvas);

    if (this.authService.authenticated) {
      console.log('is Authenticated');

      this.authService.getUserInfo().subscribe(data => {
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
    // const colourIndex = charIndex % 19;
    const colourIndex = Math.floor(Math.random() * 19);


    let canvas: HTMLCanvasElement;

    const cw = height ? height : 32; // htmlElement.clientWidth;
    const ch = cw; // htmlElement.clientHeight;

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


}








// export class AlienAvatar {
//   // Canvas
//   const canvas = new HTMLCanvasElement; // $('#random_avatar').get(0),
//   ctx;
//   px = 30;
//   px_s = 15;

//   // Download
//   // const download = $('button[role="download"]');
//   // download.click(function(e) {
//   //     var img = canvas.toDataURL("image/png");
//   //     $('img[role="result"]').attr('src', img);
//   //     $('h1.download').css('opacity', 1);
//   // });
//   // const generate = $('button[role="generate"]');
//   // generate.click(function(e) {
//   //     ravatar();
//   // });

//   ravatar() {
//     // Canvas supported
//     if (this.canvas.getContext) {
//       this.ctx = this.canvas.getContext('2d');

//       const cxlg = this.ctx.createLinearGradient(0, 0, 300, 300);
//       cxlg.addColorStop(0, '#555');
//       cxlg.addColorStop(0.5, '#ccc');
//       cxlg.addColorStop(1.0, '#666');
//       this.ctx.fillStyle = cxlg;

//       this.ctx.fillRect(0, 0, 300, 300);
//       this.ctx.fillRect(300, 0, 300, 300);
//       this.ctx.fillRect(0, 300, 300, 300);

//       this.face();

//       // Eyes
//       eyes();

//       // Mouth
//       this.draw(randomColor(), [
//         [4, 6], [5, 6]
//       ]
//       );

//       // Hair
//       hair();

//       // Body
//       body();
//     }
//   }

//   // this.ravatar();

// /**
//  * Face
//  */
// face() {
//   const faces = [
//     [ // F@ face
//       [2, 3], [3, 3], [4, 3], [5, 3], [6, 3], [7, 3.5],
//       [2, 4], [3, 4], [4, 4], [5, 4], [6, 4], [7, 4],
//       [2, 5], [3, 5], [4, 5], [5, 5], [6, 5], [7, 5],
//       [2, 6], [3, 6], [4, 6], [5, 6], [6, 6], [7, 5.5],
//     ],
//     [ // Normal face
//       [3, 3], [4, 3], [5, 3], [6, 3],
//       [3, 4], [4, 4], [5, 4], [6, 4],
//       [3, 5], [4, 5], [5, 5], [6, 5],
//       [3, 6], [4, 6], [5, 6],
//     ],
//     [ // Alien face
//       [1, 3], [2, 3], [3, 3], [4, 3], [5, 3], [6, 3], [7, 3], [8, 3],
//       [1, 4], [2, 4], [3, 4], [4, 4], [5, 4], [6, 4], [7, 4], [8, 4],
//       [3, 5], [4, 5], [5, 5], [6, 5],
//       [3, 6], [4, 6], [5, 6],
//     ]
//   ];

//   // Face
//   this.draw(randomColor(), faces[randomBetween(faces.length)]);
// }

// /**
//  * Eyes
//  */
// eyes() {
//   const eyes = [
//     [
//       [4, 4], [6, 4]
//     ]
//   ]

//   // Eyes
//   this.draw(randomColor(), eyes[randomBetween(eyes.length)]);

//   const pupil = [
//     [[4.5, 4], [6.5, 4]],
//     [[4.5, 4.5], [6.5, 4.5]],
//     [[4, 4.5], [6, 4.5]],
//     [[4, 4], [6.5, 4.5]],
//     [[4.5, 4.5], [6, 4]],
//     []
//   ];

//   // Pupil
//   this.draw(randomColor(), pupil[randomBetween(pupil.length)], px_s);
// }

// /**
//  * Hair
//  */
// hair() {
//   const hair = [
//     [
//       [4, .5], [5, .5], [6, 0],
//       [3, 1.5], [4, 1], [5, 1], [6, 1],
//       [3, 2.5], [4, 2], [5, 2], [6, 2],
//     ],
//     [
//       [4, .5], [5, .5], [6, 0], [7, 0],
//       [2, 1.5], [3, 1.5], [4, 1], [5, 1], [6, 1],
//       [2, 2.5], [3, 2.5], [4, 2], [5, 2], [6, 2], [7, 2],
//     ],
//     [
//       [4, .5], [5, .5],
//       [2, 1.5], [3, 1.5], [4, 1.5], [5, 1.5], [6, 1.5], [7, 1.5],
//       [1, 2.5], [2, 2.5], [3, 2.5], [4, 2.5], [5, 2.5], [6, 2.5], [7, 2.5], [8, 2.5]
//     ],
//     []
//   ];

//   this.draw(randomColor(), hair[randomBetween(hair.length)]);
// }

// /**
//  * Body
//  */
// body() {
//   const bodys = [
//     [
//       [2, 7], [3, 7], [4, 7], [5, 7], [6, 7],
//       [1, 8], [2, 8], [3, 8], [4, 8], [5, 8], [6, 8], [7, 8],
//       [1, 9], [2, 9], [3, 9], [4, 9], [5, 9], [6, 9], [7, 9]
//     ],
//     [
//       [2, 7], [3, 7], [4, 7], [5, 7], [5, 7], [6, 7], [7, 7],
//       [0, 8], [1, 8], [2, 8], [3, 8], [4, 8], [5, 8], [6, 8], [7, 8], [8, 8], [9, 8],
//       [0, 9], [1, 9], [2, 9], [3, 9], [4, 9], [5, 9], [6, 9], [7, 9], [8, 9], [9, 9]
//     ]
//   ];

//   // Body
//   this.draw(randomColor(), bodys[randomBetween(bodys.length)]);


//   const body_decorations = [
//     [
//       [3, 7], [5, 7], [5, 7],
//       [4, 8],
//       [4, 9],
//     ],
//     []
//   ];

//   this.draw(randomColor(), body_decorations[randomBetween(body_decorations.length)]);

//   const body_decorations_2 = [
//     [
//       [3.5, 7.5], [5, 7], [5, 7],
//       [4, 8],
//       [4, 9],
//     ],
//     [
//       [3, 8.5], [5.5, 8.5],
//       [2.5, 9], [6, 9],
//       [2.5, 9.5], [5.5, 9.5]
//     ],
//   ];

//   this.draw(randomColor(), body_decorations_2[randomBetween(body_decorations_2.length)], px_s);
// }

// draw(color, coords, size) {
//   $.each(coords, function (i, v) {

//     var _size = px;

//     if (size != undefined) {
//       _size = size;
//     }

//     ctx.fillStyle = color;
//     ctx.fillRect(coords[i][0] * px, coords[i][1] * px, _size, _size);
//   });
// }

// randomBetween(max) {
//   let r;
//   do { r = Math.random(); } while (r === 1.0);
//   return parseInt(r * max);
// }

// randomColor() {
//   return '#' + Math.floor(Math.random() * 16777215).toString(16);
// }


// }



