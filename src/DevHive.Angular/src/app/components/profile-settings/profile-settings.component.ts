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
  public dataArrived = false;
  public user: User;

  constructor(private _router: Router, private _userService: UserService)
  { }

  ngOnInit(): void {
    let username = this._router.url.substring(9);
    username = username.substring(0, username.length - 9);

    if (sessionStorage.getItem('UserCred')) {
      // Workaround for waiting the fetch response
      // TODO: properly wait for it, before loading the page contents
      setTimeout(() => {
                          this.user = this._userService.fetchUserFromSessionStorage();
      }, AppConstants.FETCH_TIMEOUT);

      // After getting the user, check if we're on the profile page of the logged in user
      setTimeout(() => {
                          if (this.user.userName !== username) {
                            this.goToProfile();
                          } else if (this.user.imageUrl === '') {
                            this.user.imageUrl = AppConstants.FALLBACK_PROFILE_ICON;
                          }
      }, AppConstants.FETCH_TIMEOUT + 50);

      setTimeout(() => {
        this.dataArrived = true;
      }, AppConstants.FETCH_TIMEOUT + 100);
    }
    else {
      this.goToProfile();
    }
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
    setTimeout(() => { this._userService.deleteUserRequest(this._userService.getUserIdFromSessionStroageToken()); }, AppConstants.FETCH_TIMEOUT);
    setTimeout(() => {
                       this._userService.logoutUserFromSessionStorage();
                       this._router.navigate(['/login']);
    }, AppConstants.FETCH_TIMEOUT + 100);
  }
}
