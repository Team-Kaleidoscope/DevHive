import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from 'src/app/services/user.service';
import { User } from 'src/models/identity/user';
import { AppConstants } from 'src/app/app-constants.module';
import { HttpErrorResponse } from '@angular/common/http';
import { Location } from '@angular/common';
import { LanguageService } from 'src/app/services/language.service';
import { TechnologyService } from 'src/app/services/technology.service';
import { Post } from 'src/models/post';
import { FeedService } from 'src/app/services/feed.service';
import { TokenService } from 'src/app/services/token.service';
import { Title } from '@angular/platform-browser';
import { Friend } from 'src/models/identity/friend';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  private _title = 'Profile';
  private _urlUsername: string;
  private _timeLoaded: string;
  private _currentPage: number;
  public isTheLoggedInUser = false;
  public isUserLoggedIn = false;
  public isAdminUser = false;
  public dataArrived = false;
  public friendOfUser = false;
  public user: User;
  public userPosts: Post[];

  constructor(private _titleService: Title, private _router: Router, private _userService: UserService, private _languageService: LanguageService, private _technologyService: TechnologyService, private _feedService: FeedService, private _location: Location, private _tokenService: TokenService) {
    this._titleService.setTitle(this._title);
  }

  private setDefaultUser(): void {
    this.user = this._userService.getDefaultUser();
  }

  ngOnInit(): void {
    this._urlUsername = this._router.url.substring(9);

    const now = new Date();
    now.setHours(now.getHours() + 2); // accounting for eastern europe timezone
    this._timeLoaded = now.toISOString();
    this._currentPage = 1;

    this.user = this._userService.getDefaultUser();
    this.userPosts = [];

    this._userService.getUserByUsernameRequest(this._urlUsername).subscribe(
      (res: object) => {
        Object.assign(this.user, res);
        this.loadLanguages();
      },
      (err: HttpErrorResponse) => {
        this._router.navigate(['/not-found']);
      }
    );
  }

  private loadLanguages(): void {
    if (this.user.languages.length > 0) {
      // When user has languages, get their names and load technologies
      this._languageService.getFullLanguagesFromIncomplete(this.user.languages).then(value => {
        this.user.languages = value;
        this.loadTechnologies();
      });
    }
    else {
      this.loadTechnologies();
    }
  }

  private loadTechnologies(): void {
    if (this.user.technologies.length > 0) {
      // When user has technologies, get their names and then load posts
      this._technologyService.getFullTechnologiesFromIncomplete(this.user.technologies).then(value => {
        this.user.technologies = value;
        this.loadPosts();
      });
    }
    else {
      this.loadPosts();
    }
  }

  private loadPosts(): void {
    this._feedService.getUserPostsRequest(this.user.userName, this._currentPage++, this._timeLoaded, AppConstants.PAGE_SIZE).subscribe(
      (result: object) => {
        const resultArr: Post[] = Object.values(result)[0];
        this.userPosts.push(...resultArr);
        this.finishUserLoading();
      },
      (err: HttpErrorResponse) => {
        this._currentPage = -1;
        this.finishUserLoading();
      }
    );
  }

  private finishUserLoading(): void {
    if (sessionStorage.getItem('UserCred')) {
      this.isUserLoggedIn = true;
      const userFromToken: User = this._userService.getDefaultUser();

      this._userService.getUserFromSessionStorageRequest().subscribe(
        (tokenRes: object) => {
          Object.assign(userFromToken, tokenRes);

          if (userFromToken.friends.map(x => x.userName).includes(this._urlUsername)) {
            this.friendOfUser = true;
          }
          if (userFromToken.userName === this._urlUsername) {
            this.isTheLoggedInUser = true;
          }
          this.dataArrived = true;
          this.isAdminUser = this.user.roles.map(x => x.name).includes(AppConstants.ADMIN_ROLE_NAME);
        },
        (err: HttpErrorResponse) => {
          this.logout();
        }
      );
    }
    else {
      this.dataArrived = true;
    }
  }

  goBack(): void {
    this._router.navigate(['/']);
  }

  navigateToAdminPanel(): void {
    this._router.navigate(['/admin-panel']);
  }

  navigateToSettings(): void {
    this._router.navigate([this._router.url + '/settings']);
  }

  logout(): void {
    this._tokenService.logoutUserFromSessionStorage();

    // Reload the page
    this._router.routeReuseStrategy.shouldReuseRoute = () => false;
    this._router.onSameUrlNavigation = 'reload';
    this._router.navigate([this._router.url]);
  }

  modifyFriend(): void {
    this.dataArrived = false;
    if (this.friendOfUser) {
      this.removeFriend();
    }
    else {
      this.addFriend();
    }
  }

  removeFriend(): void {
    this._userService.removeFriendFromUserFromSessionStorageRequest(this._urlUsername).subscribe(
      (result: object) => {
        this.reloadPage();
      },
      (err: HttpErrorResponse) => {
        this._router.navigate(['/']);
      }
    );
  }

  addFriend(): void {
    this._userService.addFriendToUserFromSessionStorageRequest(this._urlUsername).subscribe(
      (result: object) => {
        this.reloadPage();
      },
      (err: HttpErrorResponse) => {
        this._router.navigate(['/']);
      }
    );
  }

  onScroll(event: any): void {
    // Detects when the element has reached the bottom, thx https://stackoverflow.com/a/50038429/12036073
    if (event.target.offsetHeight + event.target.scrollTop >= event.target.scrollHeight && this._currentPage > 0) {
      this.loadPosts();
    }
  }

  private reloadPage(): void {
    this._router.routeReuseStrategy.shouldReuseRoute = () => false;
    this._router.onSameUrlNavigation = 'reload';
    this._router.navigate([this._router.url]);
  }
}
