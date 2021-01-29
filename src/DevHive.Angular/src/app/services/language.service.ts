import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Guid} from 'guid-typescript';
import {Observable} from 'rxjs';
import {Language} from 'src/models/identity/user';
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

  getAllLanguagesWithSessionStorageRequest(): Observable<object> {
    const token = sessionStorage.getItem('UserCred') ?? '';
    return this.getAllLanguagesRequest(token);
  }

  getAllLanguagesRequest(authToken: string): Observable<object> {
    const options = {
      headers: new HttpHeaders().set('Authorization', 'Bearer ' + authToken)
    };
    return this.http.get(AppConstants.API_LANGUAGE_URL + '/GetLanguages', options);
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
}
