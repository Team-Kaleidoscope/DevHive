import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-success-bar',
  templateUrl: './success-bar.component.html',
  styleUrls: ['./success-bar.component.css']
})
export class SuccessBarComponent implements OnInit {
  public successMsg = '';

  constructor()
  { }

  ngOnInit(): void {
    this.hideMsg();
  }

  showMsg(msg?: string | undefined): void {
    if (msg === undefined) {
      this.successMsg = 'Success!';
    }
    else if (msg.trim() === '') {
      this.successMsg = 'Success!';
    }
    else {
      this.successMsg = msg;
    }
  }

  hideMsg(): void {
    this.successMsg = '';
  }
}
