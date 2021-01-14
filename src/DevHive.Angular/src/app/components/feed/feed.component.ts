import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { FeedService } from 'src/app/services/feed.service';
import { PostComponent } from '../post/post.component';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.css']
})
export class FeedComponent implements OnInit {
  private _title = 'Feed';
  public posts: PostComponent[];

  constructor(private titleService: Title, private service: FeedService, private router: Router) {
    this.titleService.setTitle(this._title);
   }

  ngOnInit(): void {
    this.posts = [
      new PostComponent(),
      new PostComponent(),
      new PostComponent(),
      new PostComponent(),
    ];

    if (sessionStorage.getItem('UserCred')) { /* TODO: improve token validation */
      /* Make use of the JWT, will be implemented later */
    } else {
      this.router.navigate(['/login']);
    }
  }
}
