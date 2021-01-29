import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from 'src/app/services/user.service';
import { User } from 'src/models/identity/user';
import { AppConstants } from 'src/app/app-constants.module';
import {HttpErrorResponse} from '@angular/common/http';
import {Location} from '@angular/common';
import {LanguageService} from 'src/app/services/language.service';
import {TechnologyService} from 'src/app/services/technology.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  private _urlUsername: string;
  public loggedInUser = false;
  public dataArrived = false;
  public user: User;
  public showNoLangMsg = false;
  public showNoTechMsg = false;

  constructor(private _router: Router, private _userService: UserService, private _languageService: LanguageService, private _technologyService: TechnologyService, private _location: Location)
  { }

  private setDefaultUser(): void {
    this.user = this._userService.getDefaultUser();
  }

  ngOnInit(): void {
    this._urlUsername = this._router.url.substring(9);
    this.user = this._userService.getDefaultUser();

    this._userService.getUserByUsernameRequest(this._urlUsername).subscribe(
      (res: object) => this.loadLanguages(res),
      (err: HttpErrorResponse) => { this._router.navigate(['/not-found']); }
    );
  }

  private loadLanguages(res: object): void {
    Object.assign(this.user, res);

    if (this.user.languages.length > 0) {
      // For each language in the user, request it's name and assign it,
      // when you finally finish with them, start loading technologies
      const lastGuid = this.user.languages[this.user.languages.length - 1].id;
      for (const lang of this.user.languages) {
        this._languageService.getLanguageRequest(lang.id).subscribe(
          (result: object) => {
            // this only assigns the response "name" property to language
            Object.assign(lang, result);

            if (lastGuid === lang.id) {
              this.loadTechnologies();
            }
          }
        );
      }
    }
    else {
      this.showNoLangMsg = true;
      this.loadTechnologies();
    }
  }

  private loadTechnologies(): void {
    if (this.user.technologies.length > 0) {
       // For each language in the user, request it's name and assign it,
      // when you finish with them, finally finish user loading
      const lastGuid = this.user.technologies[this.user.technologies.length - 1].id;
      for (const tech of this.user.technologies) {
        this._technologyService.getTechnologyRequest(tech.id).subscribe(
          (result: object) => {
            // this only assigns the response "name" property to technology
            Object.assign(tech, result);

            if (lastGuid === tech.id) {
              this.finishUserLoading();
            }
          }
        );
      }
    }
    else {
      this.showNoTechMsg = true;
      this.finishUserLoading();
    }
  }

  private finishUserLoading(): void {
    if (this.user.imageUrl === '') {
      this.user.imageUrl = AppConstants.FALLBACK_PROFILE_ICON;
    }

    if (sessionStorage.getItem('UserCred')) {
      const userFromToken: User = this._userService.getDefaultUser();

      this._userService.getUserFromSessionStorageRequest().subscribe(
        (tokenRes: object) => {
          Object.assign(userFromToken, tokenRes);

          if (userFromToken.userName === this._urlUsername) {
            this.loggedInUser = true;
          }
          this.dataArrived = true;
        },
        (err: HttpErrorResponse) => this.bailOnBadToken()
      );
    }
    else {
      this.dataArrived = true;
    }
  }

  private bailOnBadToken(): void {
    this._userService.logoutUserFromSessionStorage();
    this._router.navigate(['/login']);
  }

  goBack(): void {
    const currURL = this._location.path();
    this._location.back();
    if (this._location.path() === currURL) {
      this._router.navigate(['/']);
    }
  }

  navigateToSettings(): void {
    this._router.navigate([this._router.url + '/settings']);
  }

  logout(): void {
    this._userService.logoutUserFromSessionStorage();

    // Reload the page
    this._router.routeReuseStrategy.shouldReuseRoute = () => false;
    this._router.onSameUrlNavigation = 'reload';
    this._router.navigate([this._router.url]);
  }
}
