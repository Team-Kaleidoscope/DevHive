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
        // Add password pattern
      ]],
    });

    this.registerUserFormGroup.valueChanges.subscribe(console.log);
  }

  onSubmit(): void {
    fetch('http://localhost:5000/api/User/register', {
      method: 'POST',
      body: `{
               "UserName": "${this.registerUserFormGroup.get('username')?.value}",
               "Email": "${this.registerUserFormGroup.get('email')?.value}",
               "FirstName": "${this.registerUserFormGroup.get('firstName')?.value}",
               "LastName": "${this.registerUserFormGroup.get('lastName')?.value}",
               "Password": "${this.registerUserFormGroup.get('password')?.value}"
      }`,
      headers: {
        'Content-Type': 'application/json'
      }
    }).then(response => response.json()).then(data => { console.log(data); });
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
