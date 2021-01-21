import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from 'src/app/services/user.service';
import { User } from 'src/models/identity/user';
import { AppConstants } from 'src/app/app-constants.module';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  public loggedInUser = false;
  public dataArrived = false;
  public user: User;

  constructor(private _router: Router, private _userService: UserService)
  { }

  private setDefaultUser(): void {
    this.user = this._userService.getDefaultUser();
  }

  ngOnInit(): void {
    const username = this._router.url.substring(9);

    if (sessionStorage.getItem('UserCred')) {
      // Workaround for waiting the fetch response
      // TODO: properly wait for it, before loading the page contents
      setTimeout(() => {
                          this.user = this._userService.fetchUserFromSessionStorage();
      }, AppConstants.FETCH_TIMEOUT);

      // After getting the user, check if we're on the profile page of the logged in user
      setTimeout(() => {
                          if (this.user.userName !== username) {
                            this.setDefaultUser();
                          } else {
                            if (this.user.imageUrl === '') {
                              this.user.imageUrl = AppConstants.FALLBACK_PROFILE_ICON;
                            }
                            this.loggedInUser = true;
                          }
      }, AppConstants.FETCH_TIMEOUT + 50);
    }
    else {
      this.setDefaultUser();
    }

    setTimeout(() => {
      this.dataArrived = true;
    }, AppConstants.FETCH_TIMEOUT + 100);
  }

  goBack(): void {
    this._router.navigate(['/']);
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
