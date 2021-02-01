import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Guid } from 'guid-typescript';
import { Observable } from 'rxjs';
import { Technology } from 'src/models/technology';
import { AppConstants } from '../app-constants.module';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class TechnologyService {
  constructor(private _http: HttpClient, private _tokenService: TokenService)
  { }

  /* Requests from session storage */

   createTechnologyWithSessionStorageRequest(name: string): Observable<object> {
    const token = this._tokenService.getTokenFromSessionStorage();

    return this.createtTechnologyRequest(name, token);
  }

  getAllTechnologiesWithSessionStorageRequest(): Observable<object> {
    const token = this._tokenService.getTokenFromSessionStorage();

    return this.getAllTechnologiesRequest(token);
  }

  putTechnologyWithSessionStorageRequest(langId: Guid, newName: string): Observable<object> {
    const token = this._tokenService.getTokenFromSessionStorage();

    return this.putTechnologyRequest(token, langId, newName);
  }

  deleteTechnologyWithSessionStorageRequest(langId: Guid): Observable<object> {
    const token = this._tokenService.getTokenFromSessionStorage();

    return this.deleteTechnologyRequest(token, langId);
  }

  /* Technology requests */

  createtTechnologyRequest(name: string, authToken: string): Observable<object> {
    const body = {
      name: name
    };
    const options = {
      headers: new HttpHeaders().set('Authorization', 'Bearer ' + authToken)
    };
    return this._http.post(AppConstants.API_TECHNOLOGY_URL, body, options);
  }

  getTechnologyRequest(techId: Guid): Observable<object> {
    const options = {
      params: new HttpParams().set('Id', techId.toString())
    };
    return this._http.get(AppConstants.API_TECHNOLOGY_URL, options);
  }

  getAllTechnologiesRequest(authToken: string): Observable<object> {
    const options = {
      headers: new HttpHeaders().set('Authorization', 'Bearer ' + authToken)
    };
    return this._http.get(AppConstants.API_TECHNOLOGY_URL + '/GetTechnologies', options);
  }

  getFullTechnologiesFromIncomplete(givenTechnologies: Technology[]): Promise<Technology[]> {
    if (givenTechnologies.length === 0) {
      return new Promise(resolve => resolve(givenTechnologies));
    }

    // This accepts language array with incomplete languages, meaning
    // languages that only have an id, but no name
    return new Promise(resolve => {
      const lastGuid = givenTechnologies[givenTechnologies.length - 1].id;

      // For each language, request his name and assign it
      for (const tech of givenTechnologies) {
        this.getTechnologyRequest(tech.id).subscribe(
          (result: object) => {
            // this only assigns the "name" property to the language,
            // because only the name is returned from the request
            Object.assign(tech, result);

            if (lastGuid === tech.id) {
              resolve(givenTechnologies);
            }
          }
        );
      }
    });
  }

  putTechnologyRequest(authToken: string, langId: Guid, newName: string): Observable<object> {
    const body = {
      name: newName
    };
    const options = {
      params: new HttpParams().set('Id', langId.toString()),
      headers: new HttpHeaders().set('Authorization', 'Bearer ' + authToken)
    };
    return this._http.put(AppConstants.API_TECHNOLOGY_URL, body, options);
  }

  deleteTechnologyRequest(authToken: string, langId: Guid): Observable<object> {
    const options = {
      params: new HttpParams().set('Id', langId.toString()),
      headers: new HttpHeaders().set('Authorization', 'Bearer ' + authToken)
    };
    return this._http.delete(AppConstants.API_TECHNOLOGY_URL, options);
  }
}
