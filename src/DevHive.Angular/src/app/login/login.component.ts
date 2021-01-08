import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, FormBuilder } from '@angular/forms';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;

  private _title = 'Login';

  constructor(private titleService: Title, private fb: FormBuilder) {
    titleService.setTitle(this._title);
  }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      userName: '',
      password: ''
    });
  }

  async onSubmit(): Promise<void> {
    const response = await fetch('http://localhost:5000/api/User/login', {
      method: 'POST',
      body: `{"UserName": "${this.loginForm.get('userName')?.value}", "Password": "${this.loginForm.get('password')?.value}"}`,
      headers: {
        'Content-Type': 'application/json'
      }
    });
    console.log(response);
  }
}
