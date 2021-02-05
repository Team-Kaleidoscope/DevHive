import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Guid } from 'guid-typescript';
import { Observable } from 'rxjs';
import { Comment } from 'src/models/comment';
import { AppConstants } from '../app-constants.module';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  constructor(private _http: HttpClient, private _tokenService: TokenService)
  { }

  getDefaultComment(): Comment {
    return new Comment(Guid.createEmpty(), Guid.createEmpty(), 'Gosho', 'Trapov', 'gosho_trapov', 'Your opinion on my idea?', new Date());
  }

  /* Requests from session storage */

  createCommentWithSessionStorageRequest(postId: Guid, commentMessage: string): Observable<object> {
    const userId = this._tokenService.getUserIdFromSessionStorageToken();
    const token = this._tokenService.getTokenFromSessionStorage();

    return this.createCommentRequest(userId, token, postId, commentMessage);
  }

  putCommentWithSessionStorageRequest(commentId: Guid, postId: Guid, newMessage: string): Observable<object> {
    const userId = this._tokenService.getUserIdFromSessionStorageToken();
    const token = this._tokenService.getTokenFromSessionStorage();

    return this.putCommentRequest(userId, token, commentId, postId, newMessage);
  }

  deleteCommentWithSessionStorage(commentId: Guid): Observable<object> {
    const token = this._tokenService.getTokenFromSessionStorage();

    return this.deleteCommentRequest(commentId, token);
  }

  /* Comment requests */

  createCommentRequest(userId: Guid, authToken: string, postId: Guid, commentMessage: string): Observable<object> {
    const body = {
      postId: postId.toString(),
      message: commentMessage
    };
    const options = {
      params: new HttpParams().set('UserId', userId.toString()),
      headers: new HttpHeaders().set('Authorization', 'Bearer ' + authToken)
    };
    return this._http.post(AppConstants.API_COMMENT_URL, body, options);
  }

  getCommentRequest(id: Guid): Observable<object> {
    const options = {
      params: new HttpParams().set('Id', id.toString())
    };
    return this._http.get(AppConstants.API_COMMENT_URL, options);
  }

  putCommentRequest(userId: Guid, authToken: string, commentId: Guid, postId: Guid, newMessage: string): Observable<object> {
    const body = {
      commentId: commentId.toString(),
      postId: postId.toString(),
      newMessage: newMessage
    };
    const options = {
      params: new HttpParams().set('UserId', userId.toString()),
      headers: new HttpHeaders().set('Authorization', 'Bearer ' + authToken)
    };
    return this._http.put(AppConstants.API_COMMENT_URL, body, options);
  }

  deleteCommentRequest(commentId: Guid, authToken: string): Observable<object> {
    const options = {
      params: new HttpParams().set('Id', commentId.toString()),
      headers: new HttpHeaders().set('Authorization', 'Bearer ' + authToken)
    };
    return this._http.delete(AppConstants.API_COMMENT_URL, options);
  }
}
