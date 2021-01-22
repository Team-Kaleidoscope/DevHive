import { Component, ErrorHandler, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl, AbstractControl } from '@angular/forms';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { UserService } from 'src/app/services/user.service';
import {AppConstants} from 'src/app/app-constants.module';
import {HttpErrorResponse, HttpResponse} from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  private _title = 'Login';
  errorMsg = ''
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
    this._userService.loginUserRequest(this.loginUserFormGroup).subscribe(
        res => this.finishLogin(res),
        (err: HttpErrorResponse) => this.showError(err)
    );
  }

  private finishLogin(res: object): void {
    this._userService.setUserTokenToSessionStorage(res);
    this._router.navigate(['/']);
  }

  private showError(error: HttpErrorResponse): void {
    this.errorMsg = error.message;
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
