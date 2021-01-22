import {HttpErrorResponse} from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import {Router} from '@angular/router';
import {AppConstants} from 'src/app/app-constants.module';
import {UserService} from 'src/app/services/user.service';
import {User} from 'src/models/identity/user';

@Component({
  selector: 'app-profile-settings',
  templateUrl: './profile-settings.component.html',
  styleUrls: ['./profile-settings.component.css']
})
export class ProfileSettingsComponent implements OnInit {
  private _urlUsername: string;
  public dataArrived = false;
  public user: User;

  constructor(private _router: Router, private _userService: UserService)
  { }

  ngOnInit(): void {
    this._urlUsername = this._router.url.substring(9)
    this._urlUsername = this._urlUsername.substring(0, this._urlUsername.length - 9);
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
            this.dataArrived = true;
          }
          else {
            this.goToProfile();
          }
        },
        (err: HttpErrorResponse) => this.bailOnBadToken()
      );
    }
    else {
      this.goToProfile();
    }
  }

  private bailOnBadToken(): void {
    this._userService.logoutUserFromSessionStorage();
    this._router.navigate(['/login']);
  }

  goToProfile(): void {
    this._router.navigate([this._router.url.substring(0, this._router.url.length - 9)]);
  }

  goBack(): void {
    this._router.navigate(['/']);
  }

  logout(): void {
    this._userService.logoutUserFromSessionStorage();
    this.goToProfile();
  }

  deleteAccount(): void {
    this._userService.deleteUserFromSessionStorageRequest().subscribe(
      (res: object) => {
        this._userService.logoutUserFromSessionStorage();
        this._router.navigate(['/login']);
      },
      (err: HttpErrorResponse) => console.log(err)
    );
  }
}
