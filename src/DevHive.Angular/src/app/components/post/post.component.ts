import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Guid } from 'guid-typescript';
import { PostService } from 'src/app/services/post.service';
import { UserService } from 'src/app/services/user.service';
import { User } from 'src/models/identity/user';
import { Post } from 'src/models/post';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css'],
})
export class PostComponent implements OnInit {
  public loaded = false;
  public user: User;
  public post: Post;
  public votesNumber: number;
  public timeCreated: string;
  @Input() paramId: string;

  constructor(private _postService: PostService, private _userService: UserService, private _router: Router)
  { }

  ngOnInit(): void {
    this.post = this._postService.getDefaultPost();
    this.user = this._userService.getDefaultUser();

    this._postService.getPostRequest(Guid.parse(this.paramId)).subscribe(
      (result: object) => {
        Object.assign(this.post, result);
        this.post.fileURLs = Object.values(result)[7];
        this.votesNumber = 23;

        this.timeCreated = new Date(this.post.timeCreated).toLocaleString('en-GB');
        this.loadUser();
      }
    );
  }

  private loadUser(): void {
    this._userService.getUserByUsernameRequest(this.post.creatorUsername).subscribe(
      (result: object) => {
        Object.assign(this.user, result);
        this.loaded = true;
      }
    );
  }

  goToAuthorProfile(): void {
    this._router.navigate(['/profile/' + this.user.userName]);
  }

  goToPostPage(): void {
    this._router.navigate(['/post/' + this.post.postId]);
  }
}
