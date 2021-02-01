import { Location } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LanguageService } from 'src/app/services/language.service';
import { UserService } from 'src/app/services/user.service';
import { TechnologyService } from 'src/app/services/technology.service';
import { User } from 'src/models/identity/user';
import { ErrorBarComponent } from '../error-bar/error-bar.component';
import { SuccessBarComponent } from '../success-bar/success-bar.component';
import { Language } from 'src/models/language';
import { Technology } from 'src/models/technology';
import { TokenService } from 'src/app/services/token.service';

@Component({
  selector: 'app-profile-settings',
  templateUrl: './profile-settings.component.html',
  styleUrls: ['./profile-settings.component.css']
})
export class ProfileSettingsComponent implements OnInit {
  private _title = 'Profile Settings';
  @ViewChild(ErrorBarComponent) private _errorBar: ErrorBarComponent;
  @ViewChild(SuccessBarComponent) private _successBar: SuccessBarComponent;
  private _urlUsername: string;
  public dataArrived = false;
  public deleteAccountConfirm = false;
  public showLanguages = false;
  public showTechnologies = false;
  public updateUserFormGroup: FormGroup;
  public user: User;
  public availableLanguages: Language[];
  public availableTechnologies: Technology[];

  constructor(private _titleService: Title, private _router: Router, private _userService: UserService, private _languageService: LanguageService, private _technologyService: TechnologyService, private _tokenService: TokenService, private _fb: FormBuilder, private _location: Location) {
    this._titleService.setTitle(this._title);
  }

  ngOnInit(): void {
    this._urlUsername = this._router.url.substring(9);
    this._urlUsername = this._urlUsername.substring(0, this._urlUsername.length - 9);

    this.user = this._userService.getDefaultUser();
    this.availableLanguages = [];
    this.availableTechnologies = [];

    this._userService.getUserByUsernameRequest(this._urlUsername).subscribe(
      (res: object) => {
        Object.assign(this.user, res);
        this.finishUserLoading();
      },
      (err: HttpErrorResponse) => {
        this._router.navigate(['/not-found']);
      }
    );

    this._languageService.getAllLanguagesWithSessionStorageRequest().subscribe(
      (result: object) => {
        this.availableLanguages = result as Language[];
      }
    );
    this._technologyService.getAllTechnologiesWithSessionStorageRequest().subscribe(
      (result: object) => {
        this.availableTechnologies = result as Technology[];
      }
    );
  }

  private finishUserLoading(): void {
    if (sessionStorage.getItem('UserCred')) {
      const userFromToken: User = this._userService.getDefaultUser();

      this._userService.getUserFromSessionStorageRequest().subscribe(
        (tokenRes: object) => {
          Object.assign(userFromToken, tokenRes);

          if (userFromToken.userName === this._urlUsername) {
            this.initForm();
            this.dataArrived = true;
          }
          else {
            this.goToProfile();
          }
        },
        (err: HttpErrorResponse) => {
          this.logout();
        }
      );
    }
    else {
      this.goToProfile();
    }
  }

  private initForm(): void {
    this.updateUserFormGroup = this._fb.group({
      firstName: new FormControl(this.user.firstName, [
        Validators.required,
        Validators.minLength(3)
      ]),
      lastName: new FormControl(this.user.lastName, [
        Validators.required,
        Validators.minLength(3)
      ]),
      username: new FormControl(this.user.userName, [
        Validators.required,
        Validators.minLength(3)
      ]),
      email: new FormControl(this.user.email, [
        Validators.required,
        Validators.email,
      ]),
      password: new FormControl('', [
        Validators.required,
        Validators.minLength(3),
        Validators.pattern('.*[0-9].*') // Check if password contains atleast one number
      ]),

      // For language we have two different controls,
      // the first one is used for input, the other one for sending data
      // because if we edit the control for input,
      // we're also gonna change the input field in the HTML
      languageInput: new FormControl(''), // The one for input
      languages: new FormControl(''), // The one that is sent

      // For technologies it's the same as it is with languages
      technologyInput: new FormControl(''),
      technologies: new FormControl('')
    });

    this.getLanguagesForShowing().then(value => {
        this.updateUserFormGroup.patchValue({ languageInput : value });
    });

    this.getTechnologiesForShowing().then(value => {
      this.updateUserFormGroup.patchValue({ technologyInput : value });
    });

    this.updateUserFormGroup.valueChanges.subscribe(() => {
      this._successBar?.hideMsg();
      this._errorBar?.hideError();
    });
  }

  private getLanguagesForShowing(): Promise<string> {
    return new Promise(resolve => {
      this._languageService.getFullLanguagesFromIncomplete(this.user.languages).then(value => {
        this.user.languages = value;
        resolve(value.map(x => x.name).join(' '));
      });
    });
  }

  private getTechnologiesForShowing(): Promise<string> {
    return new Promise(resolve => {
      this._technologyService.getFullTechnologiesFromIncomplete(this.user.technologies).then(value => {
        this.user.technologies = value;
        resolve(value.map(x => x.name).join(' '));
      });
    });
  }

  onSubmit(): void {
    this._successBar.hideMsg();
    this._errorBar.hideError();

    this.patchLanguagesControl();
    this.patchTechnologiesControl();

    this._userService.putUserFromSessionStorageRequest(this.updateUserFormGroup, this.user.roles, this.user.friends).subscribe(
        res => {
          this._successBar.showMsg('Profile updated successfully!');
        },
        (err: HttpErrorResponse) => {
          this._errorBar.showError(err);
        }
    );
  }

  private patchLanguagesControl(): void {
    // Get user input
    const langControl = this.updateUserFormGroup.get('languageInput')?.value as string ?? '';

    if (langControl === '') {
      // Add the data to the form (to the value that is going to be sent)
      this.updateUserFormGroup.patchValue({
        languages : []
      });
    }
    else {
      const names = langControl.split(' ');

      // Transfer user input to objects of type { "name": "value" }
      const actualLanguages = [];
      for (const lName of names) {
        if (lName !== '') {
          actualLanguages.push({ name : lName });
        }
      }

      // Add the data to the form (to the value that is going to be sent)
      this.updateUserFormGroup.patchValue({
        languages : actualLanguages
      });
    }
  }

  private patchTechnologiesControl(): void {
    // Get user input
    const techControl = this.updateUserFormGroup.get('technologyInput')?.value as string ?? '';

    if (techControl === '') {
      // Add the data to the form (to the value that is going to be sent)
      this.updateUserFormGroup.patchValue({
        technologies : []
      });
    }
    else {
      const names = techControl.split(' ');

      // Transfer user input to objects of type { "name": "value" }
      const actualTechnologies = [];
      for (const tName of names) {
        if (tName !== '') {
          actualTechnologies.push({ name : tName });
        }
      }

      // Add the data to the form (to the value that is going to be sent)
      this.updateUserFormGroup.patchValue({
        technologies : actualTechnologies
      });
    }
  }

  goToProfile(): void {
    this._router.navigate([this._router.url.substring(0, this._router.url.length - 9)]);
  }

  logout(): void {
    this._tokenService.logoutUserFromSessionStorage();
    this.goToProfile();
  }

  toggleLanguages(): void {
    this.showLanguages = !this.showLanguages;
  }

  toggleTechnologies(): void {
    this.showTechnologies = !this.showTechnologies;
  }

  deleteAccount(): void {
    if (this.deleteAccountConfirm) {
      this._userService.deleteUserFromSessionStorageRequest().subscribe(
        (res: object) => {
          this.logout();
        },
        (err: HttpErrorResponse) => {
          this._errorBar.showError(err);
        }
      );
    }
    else {
      this.deleteAccountConfirm = true;
    }
  }
}
