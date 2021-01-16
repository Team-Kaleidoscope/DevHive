import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  public registerUserFormGroup: FormGroup;

  private _title = 'Register';

  constructor(private titleService: Title, private fb: FormBuilder, private router: Router, private userService: UserService) {
    titleService.setTitle(this._title);
  }

  ngOnInit(): void {
    this.registerUserFormGroup = this.fb.group({
      firstName: new FormControl('', [
        Validators.required,
        Validators.minLength(3)
      ]),
      lastName: new FormControl('', [
        Validators.required,
        Validators.minLength(3)
      ]),
      username: new FormControl('', [
        Validators.required,
        Validators.minLength(3)
      ]),
      email: new FormControl('', [
        Validators.required,
        Validators.email,
      ]),
      password: new FormControl('', [
        Validators.required,
        Validators.minLength(3),
        Validators.pattern('.*[0-9].*') // Check if password contains atleast one number
      ]),
    });

    this.registerUserFormGroup.valueChanges.subscribe(console.log);
  }

  onSubmit(): void {
    this.userService.registerUser(this.registerUserFormGroup);
    this.router.navigate(['/']);
  }

  onRedirectRegister(): void {
    this.router.navigate(['/login']);
  }

  get firstName() {
    return this.registerUserFormGroup.get('firstName');
  }

  get lastName() {
    return this.registerUserFormGroup.get('lastName');
  }

  get username() {
    return this.registerUserFormGroup.get('username');
  }

  get email() {
    return this.registerUserFormGroup.get('email');
  }

  get password() {
    return this.registerUserFormGroup.get('password');
  }
}
