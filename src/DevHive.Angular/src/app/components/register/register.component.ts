import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  public registerUserFormGroup: FormGroup;

  private _title = 'Register';

  constructor(private titleService: Title, private fb: FormBuilder) {
    titleService.setTitle(this._title);
  }

  ngOnInit(): void {
    this.registerUserFormGroup = this.fb.group({
      firstName: ['', [
        Validators.required,
        Validators.minLength(3)
      ]],
      lastName: ['', [
        Validators.required,
        Validators.minLength(3)
      ]],
      age: [null, [
        Validators.required,
        Validators.min(14),
        Validators.max(120),
      ]],
      username: ['', [
        Validators.required,
        Validators.minLength(3)
      ]],
      email: ['', [
        Validators.required,
        Validators.email,
      ]],
      password: ['', [
        Validators.required,
        //Add password pattern
      ]],
    });

    this.registerUserFormGroup.valueChanges.subscribe(console.log);
  }

  get firstName() {
    return this.registerUserFormGroup.get('firstName');
  }

  get lastName() {
    return this.registerUserFormGroup.get('lastName');
  }

  get age() {
    return this.registerUserFormGroup.get('age');
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