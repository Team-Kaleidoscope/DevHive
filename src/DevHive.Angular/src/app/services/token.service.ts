import { Injectable } from '@angular/core';
import { Guid } from 'guid-typescript';
import jwt_decode from 'jwt-decode';
import { IJWTPayload } from 'src/interfaces/jwt-payload';
import { IUserCredentials } from 'src/interfaces/user-credentials';
import { AppConstants } from '../app-constants.module';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  constructor()
  { }

  /* Session storage */

  setUserTokenToSessionStorage(response: object): void {
    const token = JSON.stringify(response);
    sessionStorage.setItem(AppConstants.SESSION_TOKEN_KEY, token.substr(10, token.length - 12));
  }

  getTokenFromSessionStorage(): string {
    return sessionStorage.getItem(AppConstants.SESSION_TOKEN_KEY) ?? '';
  }

  getUserIdFromSessionStorageToken(): Guid {
    const jwt: IJWTPayload = {
      token: this.getTokenFromSessionStorage()
    };
    const userCred = jwt_decode<IUserCredentials>(jwt.token);

    return userCred.ID;
  }

  getUsernameFromSessionStorageToken(): string {
    const jwt: IJWTPayload = {
      token: this.getTokenFromSessionStorage()
    };
    const userCred = jwt_decode<IUserCredentials>(jwt.token);

    return userCred.Username;
  }

  logoutUserFromSessionStorage(): void {
    sessionStorage.removeItem(AppConstants.SESSION_TOKEN_KEY);
  }
}
