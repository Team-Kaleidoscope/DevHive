import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
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

  constructor(private titleService: Title, private service: FeedService) {
    this.titleService.setTitle(this._title);
   }

  ngOnInit(): void {
    this.posts = [
      new PostComponent(),
      new PostComponent(),
      new PostComponent(),
      new PostComponent(),
    ]
  }
}
