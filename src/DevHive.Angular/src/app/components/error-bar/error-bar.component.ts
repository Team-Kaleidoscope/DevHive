import {HttpErrorResponse} from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-error-bar',
  templateUrl: './error-bar.component.html',
  styleUrls: ['./error-bar.component.css']
})
export class ErrorBarComponent implements OnInit {
  public errorMsg = '';

  constructor() { }

  ngOnInit(): void {
    this.hideError();
  }

  showError(error: HttpErrorResponse): void {
    this.errorMsg = error.statusText;
  }

  hideError(): void {
    this.errorMsg = '';
  }
}
