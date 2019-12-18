import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-error4',
  templateUrl: './error4.component.html',
  styleUrls: ['./error4.component.scss'],
})
export class Error4Component implements OnInit {
  ngOnInit(): void {
    console.log(this.route);
    console.log(this.router);
  }
  constructor(
    private route: ActivatedRoute, private router: Router) {
  }

}
