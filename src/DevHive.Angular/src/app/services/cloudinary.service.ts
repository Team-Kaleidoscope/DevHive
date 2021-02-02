import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';

@Injectable({
 providedIn: 'root'
})
export class CloudinaryService {
  constructor(private _http: HttpClient)
  { }

  getFileRequest(fileLink: string): Observable<Blob> {
    return this._http.get(fileLink, {
      responseType: 'blob'
    });
  }
}
