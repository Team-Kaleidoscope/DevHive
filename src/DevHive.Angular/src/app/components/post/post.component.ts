import { Component, Input, OnInit } from '@angular/core';
import { Guid } from 'guid-typescript';
import {AppConstants} from 'src/app/app-constants.module';
import {FeedService} from 'src/app/services/feed.service';
import {PostService} from 'src/app/services/post.service';
import {UserService} from 'src/app/services/user.service';
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
  public timeCreated: string;
  public loaded = false;
  @Input() paramId: string;

  constructor(private _postService: PostService, private _userService: UserService)
  {}

  ngOnInit(): void {
    this.post = this._postService.getDefaultPost();
    this.user = this._userService.getDefaultUser();

    this._postService.getPostRequest(Guid.parse(this.paramId)).subscribe(
      (result: object) => {
        Object.assign(this.post, result);
        this.timeCreated = new Date(this.post.timeCreated).toLocaleString('en-GB');
        this.loadUser();
      }
    );
    this.votesNumber = 23;
  }

  private loadUser(): void {
    this._userService.getUserByUsernameRequest(this.post.creatorUsername).subscribe(
      (result: object) => {
        Object.assign(this.user, result);
        this.loaded = true;
      }
    );
  }
}
