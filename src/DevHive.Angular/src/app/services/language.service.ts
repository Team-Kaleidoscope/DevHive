import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Guid} from 'guid-typescript';
import {Observable} from 'rxjs';
import {AppConstants} from '../app-constants.module';

@Injectable({
  providedIn: 'root'
})
export class LanguageService {
  constructor(private http: HttpClient) { }

  getLanguageRequest(langId: Guid): Observable<object> {
    const options = {
      params: new HttpParams().set('Id', langId.toString()),
    };
    return this.http.get(AppConstants.API_LANGUAGE_URL, options);
  }
}
