import { Injectable } from '@angular/core';
import { Guid } from 'guid-typescript';
import { User } from '../../models/identity/user';
import { FormGroup } from '@angular/forms';
import { AppConstants } from 'src/app/app-constants.module';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Role } from 'src/models/identity/role';
import { Friend } from 'src/models/identity/friend';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private _http: HttpClient, private _tokenService: TokenService)
  { }

  getDefaultUser(): User {
    return new User(Guid.createEmpty(), 'gosho_trapov', 'Gosho', 'Trapov', 'gotra@bg.com', AppConstants.FALLBACK_PROFILE_ICON, [], [], [], []);
  }

  /* Requests from session storage */

  getUserFromSessionStorageRequest(): Observable<object> {
    const userId = this._tokenService.getUserIdFromSessionStorageToken();
    const token = this._tokenService.getTokenFromSessionStorage();

    return this.getUserRequest(userId, token);
  }

  putUserFromSessionStorageRequest(updateUserFormGroup: FormGroup, userRoles: Role[], userFriends: Friend[]): Observable<object> {
    const userId = this._tokenService.getUserIdFromSessionStorageToken();
    const token = this._tokenService.getTokenFromSessionStorage();

    return this.putUserRequest(userId, token, updateUserFormGroup, userRoles, userFriends);
  }

  deleteUserFromSessionStorageRequest(): Observable<object> {
    const userId = this._tokenService.getUserIdFromSessionStorageToken();
    const token = this._tokenService.getTokenFromSessionStorage();

    return this.deleteUserRequest(userId, token);
  }

  /* User requests */

  loginUserRequest(loginUserFormGroup: FormGroup): Observable<object> {
    const body = {
      UserName: loginUserFormGroup.get('username')?.value,
      Password: loginUserFormGroup.get('password')?.value
    };
    return this._http.post(AppConstants.API_USER_LOGIN_URL, body);
  }

  registerUserRequest(registerUserFormGroup: FormGroup): Observable<object> {
    const body = {
      UserName: registerUserFormGroup.get('username')?.value,
      Email: registerUserFormGroup.get('email')?.value,
      FirstName: registerUserFormGroup.get('firstName')?.value,
      LastName: registerUserFormGroup.get('lastName')?.value,
      Password: registerUserFormGroup.get('password')?.value
    };
    return this._http.post(AppConstants.API_USER_REGISTER_URL, body);
  }

  getUserRequest(userId: Guid, authToken: string): Observable<object> {
    const options = {
      params: new HttpParams().set('Id', userId.toString()),
      headers: new HttpHeaders().set('Authorization', 'Bearer ' + authToken)
    };
    return this._http.get(AppConstants.API_USER_URL, options);
  }

  getUserByUsernameRequest(username: string): Observable<object> {
    const options = {
      params: new HttpParams().set(AppConstants.SESSION_TOKEN_KEY, username),
    };
    return this._http.get(AppConstants.API_USER_URL + '/GetUser', options);
  }

  putUserRequest(userId: Guid, authToken: string, updateUserFormGroup: FormGroup, userRoles: Role[], userFriends: Friend[]): Observable<object> {
    const body = {
      UserName: updateUserFormGroup.get('username')?.value,
      Email: updateUserFormGroup.get('email')?.value,
      FirstName: updateUserFormGroup.get('firstName')?.value,
      LastName: updateUserFormGroup.get('lastName')?.value,
      Password: updateUserFormGroup.get('password')?.value,
      Roles: userRoles,
      Friends: userFriends,
      Languages: updateUserFormGroup.get('languages')?.value,
      Technologies: updateUserFormGroup.get('technologies')?.value
    };
    const options = {
      params: new HttpParams().set('Id', userId.toString()),
      headers: new HttpHeaders().set('Authorization', 'Bearer ' + authToken)
    };
    return this._http.put(AppConstants.API_USER_URL, body, options);
  }

  deleteUserRequest(userId: Guid, authToken: string): Observable<object> {
    const options = {
      params: new HttpParams().set('Id', userId.toString()),
      headers: new HttpHeaders().set('Authorization', 'Bearer ' + authToken)
    };
    return this._http.delete(AppConstants.API_USER_URL, options);
  }
}
