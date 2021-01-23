import { Component, ErrorHandler, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl, AbstractControl } from '@angular/forms';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { UserService } from 'src/app/services/user.service';
import {AppConstants} from 'src/app/app-constants.module';
import {HttpErrorResponse, HttpResponse} from '@angular/common/http';
import {ErrorBarComponent} from '../error-bar/error-bar.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  @ViewChild(ErrorBarComponent) private _errorBar: ErrorBarComponent;
  private _title = 'Login';
  loginUserFormGroup: FormGroup;

  constructor(private _titleService: Title, private _fb: FormBuilder, private _router: Router, private _userService: UserService) {
    this._titleService.setTitle(this._title);
  }

  ngOnInit(): void {
    this.loginUserFormGroup = this._fb.group({
      username: new FormControl('', [
        Validators.required
      ]),
      password: new FormControl('', [
        Validators.required
      ])
    });
  }

  onSubmit(): void {
    this._errorBar.hideError();
    this._userService.loginUserRequest(this.loginUserFormGroup).subscribe(
        res => this.finishLogin(res),
        (err: HttpErrorResponse) => this._errorBar.showError(err)
    );
  }

  private finishLogin(res: object): void {
    this._userService.setUserTokenToSessionStorage(res);
    this._router.navigate(['/']);
  }

  onRedirectRegister(): void {
    this._router.navigate(['/register']);
  }

  get username(): AbstractControl | null  {
    return this.loginUserFormGroup.get('username');
  }

  get password(): AbstractControl | null  {
    return this.loginUserFormGroup.get('password');
  }
}
