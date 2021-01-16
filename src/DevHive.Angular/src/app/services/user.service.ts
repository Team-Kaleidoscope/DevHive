import { Injectable } from '@angular/core';
import {Guid} from 'guid-typescript';
import { User } from '../../models/identity/user';
import jwt_decode from 'jwt-decode';
import { IJWTPayload } from '../../interfaces/jwt-payload';
import { IUserCredentials } from '../../interfaces/user-credentials';
import { FormGroup } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor() { }

  fetchUserFromSessionStorage(): User {
    // Get the token and userid from session storage
    const jwt: IJWTPayload = JSON.parse(sessionStorage.getItem('UserCred') ?? '');
    const userCred = jwt_decode<IUserCredentials>(jwt.token);

    return this.fetchUser(userCred.ID, jwt.token);
  }

  fetchUser(userId: Guid, authToken: string): User {
    const fetchedUser: User = new User(Guid.createEmpty(), '', '', '', '');

    // Fetch the data and assign it to fetchedUser
    fetch(`http://localhost:5000/api/User?Id=${userId}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + authToken
      }
    }).then(response => response.json())
      .then(data => Object.assign(fetchedUser, data));

    return fetchedUser;
  }

  // TODO: make return bool when the response is an error
  loginUser(loginUserFormGroup: FormGroup): void {
    // Save the fetch reponse in the sessionStorage
    fetch('http://localhost:5000/api/User/login', {
      method: 'POST',
      body: JSON.stringify({
        UserName: loginUserFormGroup.get('username')?.value,
        Password: loginUserFormGroup.get('password')?.value
      }),
      headers: {
        'Content-Type': 'application/json'
      }
    }).then(response => response.json())
      .then(data => sessionStorage.setItem('UserCred', JSON.stringify(data)));
  }

  // TODO: make return bool when the response is an error
  registerUser(registerUserFormGroup: FormGroup): void {
    // TODO: add a check for form data validity

    // Like in login, save the fetch reponse in the sessionStorage
    fetch('http://localhost:5000/api/User/register', {
      method: 'POST',
      body: JSON.stringify({
        UserName: registerUserFormGroup.get('username')?.value,
        Email: registerUserFormGroup.get('email')?.value,
        FirstName: registerUserFormGroup.get('firstName')?.value,
        LastName: registerUserFormGroup.get('lastName')?.value,
        Password: registerUserFormGroup.get('password')?.value
      }),
      headers: {
        'Content-Type': 'application/json'
      }
    }).then(response => response.json())
      .then(data => sessionStorage.setItem('UserCred', JSON.stringify(data)));
  }
}
