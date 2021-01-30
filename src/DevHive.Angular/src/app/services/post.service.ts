import {HttpClient, HttpParams} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Guid} from 'guid-typescript';
import {Observable} from 'rxjs';
import {Post} from 'src/models/post';
import {AppConstants} from '../app-constants.module';

@Injectable({
  providedIn: 'root'
})
export class PostService {
  constructor(private http: HttpClient) { }

  getDefaultPost(): Post {
    return new Post(Guid.createEmpty(), '', new Date());
  }

  getPostRequest(id: Guid): Observable<object> {
    const options = {
      params: new HttpParams().set('Id', id.toString())
    };
    return this.http.get(AppConstants.API_POST_URL, options);
  }
}

