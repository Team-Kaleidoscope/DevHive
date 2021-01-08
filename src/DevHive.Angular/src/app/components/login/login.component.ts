import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginUserFormGroup: FormGroup;

  private _title = 'Login';

  constructor(private titleService: Title, private fb: FormBuilder) {
    titleService.setTitle(this._title);
  }

  ngOnInit(): void {
    this.loginUserFormGroup = this.fb.group({
      username: ['', [
        Validators.required,
        Validators.minLength(3)
      ]],
      password: ['', [
        Validators.required,
        // Add password pattern
      ]]
    });
  }

  onSubmit(): void {
    fetch('http://localhost:5000/api/User/login', {
      method: 'POST',
      body: `{
               "UserName": "${this.loginUserFormGroup.get('username')?.value}",
               "Password": "${this.loginUserFormGroup.get('password')?.value}"
      }`,
      headers: {
        'Content-Type': 'application/json'
      }
    }).then(response => response.json()).then(data => { console.log(data); });
  }
}
