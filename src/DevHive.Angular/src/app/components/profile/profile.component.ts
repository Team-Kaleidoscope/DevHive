import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Guid } from 'guid-typescript';
import { UserService } from 'src/app/services/user.service';
import { User } from 'src/models/identity/user';
import { AppConstants } from 'src/app/app-constants.module';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  public dataArrived = false;
  public user: User;

  constructor(private _router: Router, private _userService: UserService)
  { }

  ngOnInit(): void {
    const username = this._router.url.substring(9);
    console.log(username);
      // Workaround for waiting the fetch response
      // TODO: properly wait for it, before loading the page contents
    // setTimeout(() => { this.user = this._userService.fetchUserFromSessionStorage(); }, AppConstants.FETCH_TIMEOUT);
    // setTimeout(() => { this.dataArrived = true; }, AppConstants.FETCH_TIMEOUT + 100);
  }
}
