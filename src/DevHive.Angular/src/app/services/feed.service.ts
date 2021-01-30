import {HttpClient, HttpParams} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {AppConstants} from '../app-constants.module';

@Injectable({
  providedIn: 'root'
})
export class FeedService {
  constructor(private http: HttpClient) { }

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
