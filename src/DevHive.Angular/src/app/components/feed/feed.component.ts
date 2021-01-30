import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { User } from 'src/models/identity/user';
import { UserService } from '../../services/user.service';
import { AppConstants } from 'src/app/app-constants.module';
import {HttpErrorResponse} from '@angular/common/http';
import {FeedService} from 'src/app/services/feed.service';
import {Post} from 'src/models/post';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.css']
})
export class FeedComponent implements OnInit {
  private _title = 'Feed';
  private _timeLoaded: string;
  public dataArrived = false;
  public user: User;
  public posts: Post[] = [];

  constructor(private _titleService: Title, private _router: Router, private _userService: UserService, private _feedService: FeedService) {
    this._titleService.setTitle(this._title);
  }

  ngOnInit(): void {
    this.user = this._userService.getDefaultUser();
    const now = new Date();
    now.setHours(now.getHours() + 2); // accounting for eastern european timezone
    this._timeLoaded = now.toISOString();

    if (sessionStorage.getItem('UserCred')) {
      this._userService.getUserFromSessionStorageRequest().subscribe(
        (res: object) => this.loadFeed(res),
        (err: HttpErrorResponse) => this.bailOnBadToken()
      );
    } else {
      this._router.navigate(['/login']);
    }
  }

  private loadFeed(res: object): void {
    Object.assign(this.user, res);

    this._feedService.getUserFeedFromSessionStorageRequest(1, this._timeLoaded, AppConstants.PAGE_SIZE).subscribe(
      (result: object) => {
        this.posts = Object.values(result)[0];
        this.finishUserLoading();
      }
    );
  }

  private finishUserLoading(): void {
    if (this.user.imageUrl === '') {
      this.user.imageUrl = AppConstants.FALLBACK_PROFILE_ICON;
    }
    this.dataArrived = true;
  }

  private bailOnBadToken(): void {
    this._userService.logoutUserFromSessionStorage();
    this._router.navigate(['/login']);
  }

  goToProfile(): void {
    this._router.navigate(['/profile/' + this.user.userName]);
  }

  goToSettings(): void {
    this._router.navigate(['/profile/' + this.user.userName + '/settings']);
  }

  logout(): void {
    this._userService.logoutUserFromSessionStorage();
    this._router.navigate(['/login']);
  }
}
