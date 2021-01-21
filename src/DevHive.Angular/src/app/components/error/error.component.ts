import { Component, OnInit } from '@angular/core';
import {Router} from '@angular/router';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.css']
})
export class ErrorComponent implements OnInit {

  constructor(private _router: Router)
  { }

  ngOnInit(): void {
  }

  backToFeed(): void {
    this._router.navigate(['/']);
  }

  backToLogin(): void {
    this._router.navigate(['/login']);
  }
}
