import { Component, Input, OnInit } from '@angular/core';
import { Guid } from 'guid-typescript';
import {AppConstants} from 'src/app/app-constants.module';
import {FeedService} from 'src/app/services/feed.service';
import {PostService} from 'src/app/services/post.service';
import { User } from 'src/models/identity/user';
import {Post} from 'src/models/post';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css'],
})
export class PostComponent implements OnInit {
  public user: User;
  public post: Post;
  public votesNumber: number;
  public loaded = false;
  @Input() paramId: string;

  constructor(private _postService: PostService)
  {}

  ngOnInit(): void {
    this.post = this._postService.getDefaultPost();
    // Fetch data in post service
    this.user = new User(
      Guid.create(),
        'gosho_trapov',
        'Gosho',
        'Trapov',
        'gotra@bg.com',
        AppConstants.FALLBACK_PROFILE_ICON,
        new Array(),
        new Array()
    );

    this._postService.getPostRequest(Guid.parse(this.paramId)).subscribe(
      (result: object) => {
        Object.assign(this.post, result);
        this.loaded = true;
      }
    );
    this.votesNumber = 23;
  }
}
