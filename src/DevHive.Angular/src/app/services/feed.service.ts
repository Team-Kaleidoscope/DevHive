import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Guid } from 'guid-typescript';
import { Observable } from 'rxjs';
import { AppConstants } from '../app-constants.module';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class FeedService {
  constructor(private _http: HttpClient, private _tokenService: TokenService)
  { }

  /* Requests from session storage */

  getUserFeedFromSessionStorageRequest(pageNumber: number, firstTimeIssued: string, pageSize: number): Observable<object> {
    const token = this._tokenService.getTokenFromSessionStorage();
    const userId = this._tokenService.getUserIdFromSessionStorageToken();

    return this.getUserFeedRequest(userId, token, pageNumber, firstTimeIssued, pageSize);
  }

  /* Feed requests */

  getUserFeedRequest(userId: Guid, authToken: string, pageNumber: number, firstTimeIssued: string, pageSize: number): Observable<object> {
    const body = {
      pageNumber: pageNumber,
      firstPageTimeIssued: firstTimeIssued,
      pageSize: pageSize
    };
    const options = {
      params: new HttpParams().set('UserId', userId.toString()),
      headers: new HttpHeaders().set('Authorization', 'Bearer ' + authToken)
    };
    return this._http.post(AppConstants.API_FEED_URL + '/GetPosts', body, options);
  }

  getUserPostsRequest(userName: string, pageNumber: number, firstTimeIssued: string, pageSize: number): Observable<object> {
    const body = {
      pageNumber: pageNumber,
      firstPageTimeIssued: firstTimeIssued,
      pageSize: pageSize
    };
    const options = {
      params: new HttpParams().set('UserName', userName)
    };
    return this._http.post(AppConstants.API_FEED_URL + '/GetUserPosts', body, options);
  }
}
