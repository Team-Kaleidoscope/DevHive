import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Guid} from 'guid-typescript';
import {Observable} from 'rxjs';
import {AppConstants} from '../app-constants.module';

@Injectable({
  providedIn: 'root'
})
export class TechnologyService {
  constructor(private http: HttpClient) { }

  getTechnologyRequest(techId: Guid): Observable<object> {
    const options = {
      params: new HttpParams().set('Id', techId.toString())
    };
    return this.http.get(AppConstants.API_TECHNOLOGY_URL, options);
  }
}
