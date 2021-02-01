import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { AppConstants } from 'src/app/app-constants.module';
import { LanguageService } from 'src/app/services/language.service';
import { TechnologyService } from 'src/app/services/technology.service';
import { TokenService } from 'src/app/services/token.service';
import { UserService } from 'src/app/services/user.service';
import { User } from 'src/models/identity/user';
import { Language } from 'src/models/language';
import { Technology } from 'src/models/technology';
import { ErrorBarComponent } from '../error-bar/error-bar.component';
import { SuccessBarComponent } from '../success-bar/success-bar.component';

@Component({
  selector: 'app-admin-panel-page',
  templateUrl: './admin-panel-page.component.html',
  styleUrls: ['./admin-panel-page.component.css']
})
export class AdminPanelPageComponent implements OnInit {
  @ViewChild(ErrorBarComponent) private _errorBar: ErrorBarComponent;
  @ViewChild(SuccessBarComponent) private _successBar: SuccessBarComponent;
  public dataArrived = false;
  public showLanguages = false;
  public showTechnologies = false;
  public availableLanguages: Language[];
  public availableTechnologies: Technology[];
  public languageForm: FormGroup;
  public technologyForm: FormGroup;

  constructor(private _router: Router, private _fb: FormBuilder, private _userService: UserService, private _languageService: LanguageService, private _technologyService: TechnologyService, private _tokenService: TokenService)
  { }

  ngOnInit(): void {
    if (!this._tokenService.getTokenFromSessionStorage()) {
      this._router.navigate(['/login']);
      return;
    }

    this._userService.getUserFromSessionStorageRequest().subscribe(
      (result: object) => {
        const user = result as User;
        if (!user.roles.map(x => x.name).includes(AppConstants.ADMIN_ROLE_NAME)) {
          this._router.navigate(['/login']);
        }
      },
      (err: HttpErrorResponse) => {
        this._router.navigate(['/login']);
      }
    );

    this.languageForm = this._fb.group({
      languageCreate: new FormControl(''),
      updateLanguageOldName: new FormControl(''),
      updateLanguageNewName: new FormControl(''),
      deleteLanguageName: new FormControl('')
    });

    this.technologyForm = this._fb.group({
      technologyCreate: new FormControl(''),
      updateTechnologyOldName: new FormControl(''),
      updateTechnologyNewName: new FormControl(''),
      deleteTechnologyName: new FormControl('')
    });


    this.loadAvailableLanguages();
    this.loadAvailableTechnologies();
  }

  // Navigation

  backToProfile(): void {
    this._router.navigate(['/profile/' + this._tokenService.getUsernameFromSessionStorageToken()]);
  }

  backToFeed(): void {
    this._router.navigate(['/']);
  }

  logout(): void {
    this._tokenService.logoutUserFromSessionStorage();
    this._router.navigate(['/login']);
  }

  // Language modifying

  toggleLanguages(): void {
    this.showLanguages = !this.showLanguages;
  }

  submitLanguages(): void {
    this.tryCreateLanguage();
    this.tryUpdateLanguage();
    this.tryDeleteLanguage();
  }

  private tryCreateLanguage(): void {
    const languageCreate: string = this.languageForm.get('languageCreate')?.value;

    if (languageCreate !== '' && languageCreate !== null) {
      this._languageService.createLanguageWithSessionStorageRequest(languageCreate.trim()).subscribe(
        (result: object) => {
          this.languageModifiedSuccess('Successfully updated languages!');
        },
        (err: HttpErrorResponse) => {
          this._errorBar.showError(err);
        }
      );
    }
  }

  private tryUpdateLanguage(): void {
    const updateLanguageOldName: string = this.languageForm.get('updateLanguageOldName')?.value;
    const updateLanguageNewName: string = this.languageForm.get('updateLanguageNewName')?.value;

    if (updateLanguageOldName !== '' && updateLanguageOldName !== null && updateLanguageNewName !== '' && updateLanguageNewName !== null) {
      const langId = this.availableLanguages.filter(x => x.name === updateLanguageOldName.trim())[0].id;

      this._languageService.putLanguageWithSessionStorageRequest(langId, updateLanguageNewName.trim()).subscribe(
        (result: object) => {
          this.languageModifiedSuccess('Successfully updated languages!');
        },
        (err: HttpErrorResponse) => {
          this._errorBar.showError(err);
        }
      );
    }
  }

  private tryDeleteLanguage(): void {
    const deleteLanguageName: string = this.languageForm.get('deleteLanguageName')?.value;

    if (deleteLanguageName !== '' && deleteLanguageName !== null) {
      const langId = this.availableLanguages.filter(x => x.name === deleteLanguageName.trim())[0].id;

      console.log(langId);
      this._languageService.deleteLanguageWithSessionStorageRequest(langId).subscribe(
        (result: object) => {
          this.languageModifiedSuccess('Successfully deleted language!');
        },
        (err: HttpErrorResponse) => {
          this._errorBar.showError(err);
        }
      );
    }
  }

  private languageModifiedSuccess(successMsg: string): void {
    this._successBar.showMsg(successMsg);
    this.loadAvailableLanguages();
    this.languageForm.reset();
  }

  private loadAvailableLanguages(): void {
    this._languageService.getAllLanguagesWithSessionStorageRequest().subscribe(
      (result: object) => {
        this.availableLanguages = result as Language[];
      }
    );
  }

  // Technology modifying

  toggleTechnologies(): void {
    this.showTechnologies = !this.showTechnologies;
  }

  submitTechnologies(): void {
    this.tryCreateTechnology();
    this.tryUpdateTechnology();
    this.tryDeleteTechnology();
  }

  private tryCreateTechnology(): void {
    const technologyCreate: string = this.technologyForm.get('technologyCreate')?.value;

    if (technologyCreate !== '' && technologyCreate !== null) {
      this._technologyService.createTechnologyWithSessionStorageRequest(technologyCreate.trim()).subscribe(
        (result: object) => {
          this.technologyModifiedSuccess('Successfully updated technologies!');
        },
        (err: HttpErrorResponse) => {
          this._errorBar.showError(err);
        }
      );
    }
  }

  private tryUpdateTechnology(): void {
    const updateTechnologyOldName: string = this.technologyForm.get('updateTechnologyOldName')?.value;
    const updateTechnologyNewName: string = this.technologyForm.get('updateTechnologyNewName')?.value;

    if (updateTechnologyOldName !== '' && updateTechnologyOldName !== null && updateTechnologyNewName !== '' && updateTechnologyNewName !== null) {
      const techId = this.availableTechnologies.filter(x => x.name === updateTechnologyOldName.trim())[0].id;

      this._technologyService.putTechnologyWithSessionStorageRequest(techId, updateTechnologyNewName.trim()).subscribe(
        (result: object) => {
          this.technologyModifiedSuccess('Successfully updated technologies!');
        },
        (err: HttpErrorResponse) => {
          this._errorBar.showError(err);
        }
      );
    }
  }

  private tryDeleteTechnology(): void {
    const deleteTechnologyName: string = this.technologyForm.get('deleteTechnologyName')?.value;

    if (deleteTechnologyName !== '' && deleteTechnologyName !== null) {
      const techId = this.availableTechnologies.filter(x => x.name === deleteTechnologyName.trim())[0].id;

      this._technologyService.deleteTechnologyWithSessionStorageRequest(techId).subscribe(
        (result: object) => {
          this.technologyModifiedSuccess('Successfully deleted technology!');
        },
        (err: HttpErrorResponse) => {
          this._errorBar.showError(err);
        }
      );
    }
  }

  private technologyModifiedSuccess(successMsg: string): void {
    this._successBar.showMsg(successMsg);
    this.loadAvailableTechnologies();
    this.technologyForm.reset();
  }

  private loadAvailableTechnologies(): void {
     this._technologyService.getAllTechnologiesWithSessionStorageRequest().subscribe(
      (result: object) => {
        this.availableTechnologies = result as Technology[];
      }
    );
  }
}
