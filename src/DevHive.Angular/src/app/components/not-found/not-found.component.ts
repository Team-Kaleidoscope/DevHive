import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrls: ['./not-found.component.css']
})
export class NotFoundComponent implements OnInit {
  private _title = 'Not Found!';

  constructor(private _titleService: Title, private _router: Router) {
    this._titleService.setTitle(this._title);
  }

  ngOnInit(): void {
  }

  backToFeed(): void {
    this._router.navigate(['/']);
  }

  backToLogin(): void {
    this._router.navigate(['/login']);
  }
}
