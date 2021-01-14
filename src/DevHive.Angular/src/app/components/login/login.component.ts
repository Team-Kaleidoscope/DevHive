import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginUserFormGroup: FormGroup;

  private _title = 'Login';

  constructor(private titleService: Title, private fb: FormBuilder, private router: Router) {
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

  async onSubmit(): Promise<void> {
    const response = await fetch('http://localhost:5000/api/User/login', {
      method: 'POST',
      body: JSON.stringify({
        UserName: this.loginUserFormGroup.get('username')?.value,
        Password: this.loginUserFormGroup.get('password')?.value
      }),
      headers: {
        'Content-Type': 'application/json'
      }
    });
    const userCred: string = await response.json();

    sessionStorage.setItem('UserCred', JSON.stringify(userCred));
    this.router.navigate(['/']);
  }

  onRedirectRegister(): void {
    this.router.navigate(['/register']);
  }
}
