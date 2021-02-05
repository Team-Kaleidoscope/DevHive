import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { IApiError } from 'src/interfaces/api-error';

@Component({
  selector: 'app-error-bar',
  templateUrl: './error-bar.component.html',
  styleUrls: ['./error-bar.component.css']
})
export class ErrorBarComponent implements OnInit {
  public errorMsg = '';

  constructor()
  { }

  ngOnInit(): void {
    this.hideError();
  }

  showError(error: HttpErrorResponse): void {
    const test: IApiError = {
      type: '',
      title: 'Error!',
      status: 0,
      traceId: ''
    };
    Object.assign(test, error.error);
    this.errorMsg = test.title;
  }

  hideError(): void {
    this.errorMsg = '';
  }
}
