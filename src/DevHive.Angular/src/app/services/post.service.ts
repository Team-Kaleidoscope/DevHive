import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Guid } from 'guid-typescript';
import { Observable } from 'rxjs';
import { Post } from 'src/models/post';
import { AppConstants } from '../app-constants.module';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class PostService {
  constructor(private _http: HttpClient, private _tokenService: TokenService)
  { }

  getDefaultPost(): Post {
    return new Post(Guid.createEmpty(), 'Gosho', 'Trapov', 'gosho_trapov', 'Your opinion on my idea?', new Date());
  }

  /* Requests from session storage */

  createPostFromSessionStorageRequest(postMessage: string): Observable<object> {
    const userId = this._tokenService.getUserIdFromSessionStorageToken();
    const token = this._tokenService.getTokenFromSessionStorage();

    return this.createPostRequest(userId, token, postMessage);
  }

  /* Post requests */

  createPostRequest(userId: Guid, authToken: string, postMessage: string): Observable<object> {
    const body = {
      message: postMessage,
      files: []
    };
    const options = {
      params: new HttpParams().set('UserId', userId.toString()),
      headers: new HttpHeaders().set('Authorization', 'Bearer ' + authToken)
    };
    return this._http.post(AppConstants.API_POST_URL, body, options);
  }

  getPostRequest(id: Guid): Observable<object> {
    const options = {
      params: new HttpParams().set('Id', id.toString())
    };
    return this._http.get(AppConstants.API_POST_URL, options);
  }
}
