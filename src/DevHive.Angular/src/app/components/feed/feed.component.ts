import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { User } from 'src/models/identity/user';
import { PostComponent } from '../post/post.component';
import { UserService } from '../../services/user.service';
import { AppConstants } from 'src/app/app-constants.module';
import {HttpErrorResponse} from '@angular/common/http';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.css']
})
export class FeedComponent implements OnInit {
  private _title = 'Feed';
  public dataArrived = false;
  public user: User;
  public posts: PostComponent[];

  constructor(private _titleService: Title, private _router: Router, private _userService: UserService) {
    this._titleService.setTitle(this._title);
  }

  ngOnInit(): void {
    this.user = this._userService.getDefaultUser();
    this.posts = [
      new PostComponent(),
      new PostComponent(),
      new PostComponent(),
      new PostComponent(),
    ];

    if (sessionStorage.getItem('UserCred')) {
      this._userService.getUserFromSessionStorageRequest().subscribe(
        (res: object) => this.finishUserLoading(res),
        (err: HttpErrorResponse) => this.bailOnBadToken()
      );
    } else {
      this._router.navigate(['/login']);
    }
  }

  private finishUserLoading(res: object): void {
    Object.assign(this.user, res);
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
