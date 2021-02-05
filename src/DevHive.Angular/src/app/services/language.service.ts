import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Guid } from 'guid-typescript';
import { Observable } from 'rxjs';
import { Language } from 'src/models/language';
import { AppConstants } from '../app-constants.module';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class LanguageService {
  constructor(private _http: HttpClient, private _tokenService: TokenService)
  { }

  /* Requests from session storage */

  createLanguageWithSessionStorageRequest(name: string): Observable<object> {
    const token = this._tokenService.getTokenFromSessionStorage();

    return this.createLanguageRequest(name, token);
  }

  getAllLanguagesWithSessionStorageRequest(): Observable<object> {
    const token = this._tokenService.getTokenFromSessionStorage();

    return this.getAllLanguagesRequest(token);
  }

  putLanguageWithSessionStorageRequest(langId: Guid, newName: string): Observable<object> {
    const token = this._tokenService.getTokenFromSessionStorage();

    return this.putLanguageRequest(token, langId, newName);
  }

  deleteLanguageWithSessionStorageRequest(langId: Guid): Observable<object> {
    const token = this._tokenService.getTokenFromSessionStorage();

    return this.deleteLanguageRequest(token, langId);
  }

  /* Language requests */

  createLanguageRequest(name: string, authToken: string): Observable<object> {
    const body = {
      name: name
    };
    const options = {
      headers: new HttpHeaders().set('Authorization', 'Bearer ' + authToken)
    };
    return this._http.post(AppConstants.API_LANGUAGE_URL, body, options);
  }

  getLanguageRequest(langId: Guid): Observable<object> {
    const options = {
      params: new HttpParams().set('Id', langId.toString()),
    };
    return this._http.get(AppConstants.API_LANGUAGE_URL, options);
  }

  getAllLanguagesRequest(authToken: string): Observable<object> {
    const options = {
      headers: new HttpHeaders().set('Authorization', 'Bearer ' + authToken)
    };
    return this._http.get(AppConstants.API_LANGUAGE_URL + '/GetLanguages', options);
  }

  getFullLanguagesFromIncomplete(givenLanguages: Language[]): Promise<Language[]> {
    if (givenLanguages.length === 0) {
      return new Promise(resolve => resolve(givenLanguages));
    }

    // This accepts language array with incomplete languages, meaning
    // languages that only have an id, but no name
    return new Promise(resolve => {
      const lastGuid = givenLanguages[givenLanguages.length - 1].id;

      // For each language, request his name and assign it
      for (const lang of givenLanguages) {
        this.getLanguageRequest(lang.id).subscribe(
          (result: object) => {
            // this only assigns the "name" property to the language,
            // because only the name is returned from the request
            Object.assign(lang, result);

            if (lastGuid === lang.id) {
              resolve(givenLanguages);
            }
          }
        );
      }
    });
  }

  putLanguageRequest(authToken: string, langId: Guid, newName: string): Observable<object> {
    const body = {
      name: newName
    };
    const options = {
      params: new HttpParams().set('Id', langId.toString()),
      headers: new HttpHeaders().set('Authorization', 'Bearer ' + authToken)
    };
    return this._http.put(AppConstants.API_LANGUAGE_URL, body, options);
  }

  deleteLanguageRequest(authToken: string, langId: Guid): Observable<object> {
    const options = {
      params: new HttpParams().set('Id', langId.toString()),
      headers: new HttpHeaders().set('Authorization', 'Bearer ' + authToken)
    };
    return this._http.delete(AppConstants.API_LANGUAGE_URL, options);
  }
}
