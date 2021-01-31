import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { User } from 'src/models/identity/user';
import { UserService } from '../../services/user.service';
import { AppConstants } from 'src/app/app-constants.module';
import { HttpErrorResponse } from '@angular/common/http';
import { FeedService } from 'src/app/services/feed.service';
import { Post } from 'src/models/post';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { PostService } from 'src/app/services/post.service';
import { TokenService } from 'src/app/services/token.service';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.css']
})
export class FeedComponent implements OnInit {
  private _title = 'Feed';
  private _timeLoaded: string; // we send the time to the api as a string
  public dataArrived = false;
  public user: User;
  public posts: Post[];
  public createPostFormGroup: FormGroup;

  constructor(private _titleService: Title, private _fb: FormBuilder, private _router: Router, private _userService: UserService, private _feedService: FeedService, private _postService: PostService, private _tokenService: TokenService) {
    this._titleService.setTitle(this._title);
  }

  ngOnInit(): void {
    this.posts = [];
    this.user = this._userService.getDefaultUser();

    const now = new Date();
    now.setHours(now.getHours() + 2); // accounting for eastern european timezone
    this._timeLoaded = now.toISOString();

    this.createPostFormGroup = this._fb.group({
      newPostMessage: new FormControl('')
    });

    if (sessionStorage.getItem('UserCred')) {
      this._userService.getUserFromSessionStorageRequest().subscribe(
        (res: object) => {
          Object.assign(this.user, res);
          this.loadFeed();
        },
        (err: HttpErrorResponse) => {
          this.logout();
        }
      );
    } else {
      this._router.navigate(['/login']);
    }
  }

  private loadFeed(): void {
    this._feedService.getUserFeedFromSessionStorageRequest(1, this._timeLoaded, AppConstants.PAGE_SIZE).subscribe(
      (result: object) => {
        this.posts = Object.values(result)[0];
        this.finishUserLoading();
      },
      (err: HttpErrorResponse) => {
        this.finishUserLoading();
      }
    );
  }

  private finishUserLoading(): void {
    this.dataArrived = true;
  }

  goToProfile(): void {
    this._router.navigate(['/profile/' + this.user.userName]);
  }

  goToSettings(): void {
    this._router.navigate(['/profile/' + this.user.userName + '/settings']);
  }

  logout(): void {
    this._tokenService.logoutUserFromSessionStorage();
    this._router.navigate(['/login']);
  }

  createPost(): void {
    const postMessage = this.createPostFormGroup.get('newPostMessage')?.value;

    this._postService.createPostFromSessionStorageRequest(postMessage).subscribe(
      (result: object) => {
        this.goToProfile();
      }
    );
  }
}
