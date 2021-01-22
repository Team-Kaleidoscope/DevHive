import {Location} from '@angular/common';
import {HttpErrorResponse} from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
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
  public updateUserFormGroup: FormGroup;
  public dataArrived = false;
  public user: User;
  public successfulUpdate = false;

  constructor(private _router: Router, private _userService: UserService, private _fb: FormBuilder, private _location: Location)
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
            this.initForm();
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

  private initForm(): void {
    this.updateUserFormGroup = this._fb.group({
      firstName: new FormControl(this.user.firstName, [
        Validators.required,
        Validators.minLength(3)
      ]),
      lastName: new FormControl(this.user.lastName, [
        Validators.required,
        Validators.minLength(3)
      ]),
      username: new FormControl(this.user.userName, [
        Validators.required,
        Validators.minLength(3)
      ]),
      email: new FormControl(this.user.email, [
        Validators.required,
        Validators.email,
      ]),
      password: new FormControl('', [
        Validators.required,
        Validators.minLength(3),
        Validators.pattern('.*[0-9].*') // Check if password contains atleast one number
      ]),
    });

    this.updateUserFormGroup.valueChanges.subscribe(() => this.successfulUpdate = false);
  }

  private bailOnBadToken(): void {
    this._userService.logoutUserFromSessionStorage();
    this._router.navigate(['/login']);
  }

  onSubmit(): void {
    this.successfulUpdate = false;
    this._userService.putUserFromSessionStorageRequest(this.updateUserFormGroup).subscribe(
        res => this.successfulUpdate = true,
        (err: HttpErrorResponse) => console.log(err.message)
    );
  }

  goToProfile(): void {
    this._router.navigate([this._router.url.substring(0, this._router.url.length - 9)]);
  }

  goBack(): void {
    const currURL = this._location.path();
    this._location.back();
    if (this._location.path() === currURL) {
      this.goToProfile();
    }
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
