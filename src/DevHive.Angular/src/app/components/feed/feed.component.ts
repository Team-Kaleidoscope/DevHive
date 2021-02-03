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
  private _currentPage: number;
  public dataArrived = false;
  public user: User;
  public posts: Post[];
  public createPostFormGroup: FormGroup;
  public files: File[];

  constructor(private _titleService: Title, private _fb: FormBuilder, private _router: Router, private _userService: UserService, private _feedService: FeedService, private _postService: PostService, private _tokenService: TokenService) {
    this._titleService.setTitle(this._title);
  }

  ngOnInit(): void {
    if (!this._tokenService.getTokenFromSessionStorage()) {
      this._router.navigate(['/login']);
      return;
    }

    this._currentPage = 1;
    this.posts = [];
    this.user = this._userService.getDefaultUser();
    this.files = [];

    const now = new Date();
    now.setHours(now.getHours() + 2); // accounting for eastern european timezone
    this._timeLoaded = now.toISOString();

    this.createPostFormGroup = this._fb.group({
      newPostMessage: new FormControl(''),
      fileUpload: new FormControl('')
    });

    this._userService.getUserFromSessionStorageRequest().subscribe(
      (res: object) => {
        Object.assign(this.user, res);
        this.loadFeed();
      },
      (err: HttpErrorResponse) => {
        this.logout();
      }
    );
  }

  private loadFeed(): void {
    this._feedService.getUserFeedFromSessionStorageRequest(this._currentPage++, this._timeLoaded, AppConstants.PAGE_SIZE).subscribe(
      (result: object) => {
        this.posts.push(...Object.values(result)[0]);
        this.finishUserLoading();
      },
      (err) => {
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

  onFileUpload(event: any): void {
    this.files.push(...event.target.files);
    this.createPostFormGroup.get('fileUpload')?.reset();
  }

  removeAttachment(fileName: string): void {
    this.files = this.files.filter(x => x.name !== fileName);
  }

  createPost(): void {
    const postMessage = this.createPostFormGroup.get('newPostMessage')?.value;

    this._postService.createPostWithSessionStorageRequest(postMessage, this.files).subscribe(
      (result: object) => {
        this.goToProfile();
      }
    );
  }

  onScroll(event: any): void {
    // Detects when the element has reached the bottom, thx https://stackoverflow.com/a/50038429/12036073
    if (event.target.offsetHeight + event.target.scrollTop >= event.target.scrollHeight && this._currentPage > 0) {
      this.loadFeed();
    }
  }
}
