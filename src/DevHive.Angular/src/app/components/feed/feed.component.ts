import { Component, OnInit } from '@angular/core';
import { FeedService } from 'src/app/services/feed.service';
import { PostComponent } from '../post/post.component';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.css']
})
export class FeedComponent implements OnInit {
  public posts: PostComponent[] = [ ];

  constructor(private service: FeedService) { }

  ngOnInit(): void {
  }
}
