import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Guid} from 'guid-typescript';
import {Observable} from 'rxjs';
import {IJWTPayload} from 'src/interfaces/jwt-payload';
import {AppConstants} from '../app-constants.module';
import jwt_decode from 'jwt-decode';
import {IUserCredentials} from 'src/interfaces/user-credentials';

@Injectable({
  providedIn: 'root'
})
export class FeedService {
  constructor(private http: HttpClient) { }

  getUserFeedFromSessionStorageRequest(pageNumber: number, firstTimeIssued: string, pageSize: number): Observable<object> {
    const jwt: IJWTPayload = { token: sessionStorage.getItem('UserCred') ?? '' };
    const userCred = jwt_decode<IUserCredentials>(jwt.token);
    return this.getUserFeedRequest(userCred.ID, jwt.token, pageNumber, firstTimeIssued, pageSize);
  }

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
    return this.http.post(AppConstants.API_FEED_URL + '/GetPosts', body, options);
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
    return this.http.post(AppConstants.API_FEED_URL + '/GetUserPosts', body, options);
  }
}
