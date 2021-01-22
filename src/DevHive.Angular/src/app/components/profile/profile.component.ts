import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from 'src/app/services/user.service';
import { User } from 'src/models/identity/user';
import { AppConstants } from 'src/app/app-constants.module';
import {HttpErrorResponse} from '@angular/common/http';
import {Location} from '@angular/common';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  private _urlUsername: string;
  public loggedInUser = false;
  public dataArrived = false;
  public user: User;

  constructor(private _router: Router, private _userService: UserService, private _location: Location)
  { }

  private setDefaultUser(): void {
    this.user = this._userService.getDefaultUser();
  }

  ngOnInit(): void {
    this._urlUsername = this._router.url.substring(9);
    this.user = this._userService.getDefaultUser();

    this._userService.getUserByUsernameRequest(this._urlUsername).subscribe(
      (res: object) => this.finishUserLoading(res),
      (err: HttpErrorResponse) => { this._router.navigate(['/error']); }
    );
  }

  private finishUserLoading(res: object): void {
    Object.assign(this.user, res);
    if (this.user.imageUrl === '') {
      this.user.imageUrl = AppConstants.FALLBACK_PROFILE_ICON;
    }

    if (sessionStorage.getItem('UserCred')) {
      const userFromToken: User = this._userService.getDefaultUser();

      this._userService.getUserFromSessionStorageRequest().subscribe(
        (tokenRes: object) => {
          Object.assign(userFromToken, tokenRes);

          if (userFromToken.userName === this._urlUsername) {
            this.loggedInUser = true;
          }
          this.dataArrived = true;
        },
        (err: HttpErrorResponse) => this.bailOnBadToken()
      );
    }
    else {
      this.dataArrived = true;
    }
  }

  private bailOnBadToken(): void {
    this._userService.logoutUserFromSessionStorage();
    this._router.navigate(['/login']);
  }

  goBack(): void {
    const currURL = this._location.path();
    this._location.back();
    if (this._location.path() === currURL) {
      this._router.navigate(['/']);
    }
  }

  navigateToSettings(): void {
    this._router.navigate([this._router.url + '/settings']);
  }

  logout(): void {
    this._userService.logoutUserFromSessionStorage();

    // Reload the page
    this._router.routeReuseStrategy.shouldReuseRoute = () => false;
    this._router.onSameUrlNavigation = 'reload';
    this._router.navigate([this._router.url]);
  }
}
