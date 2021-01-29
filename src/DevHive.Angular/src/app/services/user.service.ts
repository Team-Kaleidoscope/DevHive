import { Injectable } from '@angular/core';
import {Guid} from 'guid-typescript';
import { User } from '../../models/identity/user';
import jwt_decode from 'jwt-decode';
import { IJWTPayload } from '../../interfaces/jwt-payload';
import { IUserCredentials } from '../../interfaces/user-credentials';
import { FormGroup } from '@angular/forms';
import { AppConstants } from 'src/app/app-constants.module';
import {HttpClient, HttpErrorResponse, HttpHeaders, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http: HttpClient) { }

  getDefaultUser(): User {
    return new User(Guid.createEmpty(), 'gosho_trapov', 'Gosho', 'Trapov', 'gotra@bg.com', AppConstants.FALLBACK_PROFILE_ICON, new Array(), new Array());
  }

  getUserIdFromSessionStorageToken(): Guid {
    const jwt: IJWTPayload = { token: sessionStorage.getItem('UserCred') ?? '' };
    const userCred = jwt_decode<IUserCredentials>(jwt.token);
    return userCred.ID;
  }

  setUserTokenToSessionStorage(response: object): void {
    const token = JSON.stringify(response);
    sessionStorage.setItem('UserCred', token.substr(10, token.length - 12));
  }

  getUserFromSessionStorageRequest(): Observable<object> {
    // Get the token and userid from session storage
    const jwt: IJWTPayload = { token: sessionStorage.getItem('UserCred') ?? '' };
    const userCred = jwt_decode<IUserCredentials>(jwt.token);

    return this.getUserRequest(userCred.ID, jwt.token);
  }

  putUserFromSessionStorageRequest(updateUserFormGroup: FormGroup): Observable<object> {
    // Get the token and userid from session storage
    const jwt: IJWTPayload = { token: sessionStorage.getItem('UserCred') ?? '' };
    const userCred = jwt_decode<IUserCredentials>(jwt.token);

    return this.putUserRequest(userCred.ID, jwt.token, updateUserFormGroup);
  }

  deleteUserFromSessionStorageRequest(): Observable<object> {
    // Get the token and userid from session storage
    const jwt: IJWTPayload = { token: sessionStorage.getItem('UserCred') ?? '' };
    const userCred = jwt_decode<IUserCredentials>(jwt.token);

    return this.deleteUserRequest(userCred.ID, jwt.token);
  }

  logoutUserFromSessionStorage(): void {
    sessionStorage.removeItem('UserCred');
  }

  loginUserRequest(loginUserFormGroup: FormGroup): Observable<object> {
    const body = {
      UserName: loginUserFormGroup.get('username')?.value,
      Password: loginUserFormGroup.get('password')?.value
   };
    return this.http.post(AppConstants.API_USER_LOGIN_URL, body);
  }

  registerUserRequest(registerUserFormGroup: FormGroup): Observable<object> {
    // TODO?: add a check for form data validity
    const body = {
      UserName: registerUserFormGroup.get('username')?.value,
      Email: registerUserFormGroup.get('email')?.value,
      FirstName: registerUserFormGroup.get('firstName')?.value,
      LastName: registerUserFormGroup.get('lastName')?.value,
      Password: registerUserFormGroup.get('password')?.value
    };
    return this.http.post(AppConstants.API_USER_REGISTER_URL, body);
  }

  getUserRequest(userId: Guid, authToken: string): Observable<object> {
    const options = {
      params: new HttpParams().set('Id', userId.toString()),
      headers: new HttpHeaders().set('Authorization', 'Bearer ' + authToken)
    };
    return this.http.get(AppConstants.API_USER_URL, options);
  }

  getUserByUsernameRequest(username: string): Observable<object> {
    const options = {
      params: new HttpParams().set('UserName', username),
    };
    return this.http.get(AppConstants.API_USER_URL + '/GetUser', options);
  }

  putUserRequest(userId: Guid, authToken: string, updateUserFormGroup: FormGroup): Observable<object> {
    // TODO?: add a check for form data validity
    const body = {
      UserName: updateUserFormGroup.get('username')?.value,
      Email: updateUserFormGroup.get('email')?.value,
      FirstName: updateUserFormGroup.get('firstName')?.value,
      LastName: updateUserFormGroup.get('lastName')?.value,
      Password: updateUserFormGroup.get('password')?.value,
      // TODO: make the following fields dynamically selectable
      Roles: [ { Name: 'User' } ],
      Friends: [],
      Languages: [],
      Technologies: []
    };
    const options = {
      params: new HttpParams().set('Id', userId.toString()),
      headers: new HttpHeaders().set('Authorization', 'Bearer ' + authToken)
    };
    return this.http.put(AppConstants.API_USER_URL, body, options);
  }

  deleteUserRequest(userId: Guid, authToken: string): Observable<object> {
    const options = {
      params: new HttpParams().set('Id', userId.toString()),
      headers: new HttpHeaders().set('Authorization', 'Bearer ' + authToken)
    };
    return this.http.delete(AppConstants.API_USER_URL, options);
  }
}
