import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Guid } from 'guid-typescript';

@Component({
  selector: 'app-post-page',
  templateUrl: './post-page.component.html',
  styleUrls: ['./post-page.component.css']
})
export class PostPageComponent implements OnInit {
  public postId: Guid;

  constructor(private _router: Router)
  { }

  ngOnInit(): void {
    this.postId = Guid.parse(this._router.url.substring(6));
  }

  backToFeed(): void {
    this._router.navigate(['/']);
  }
}
