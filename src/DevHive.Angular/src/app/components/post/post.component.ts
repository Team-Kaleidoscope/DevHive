import { Component, OnInit } from '@angular/core';
import { Guid } from 'guid-typescript';
import {AppConstants} from 'src/app/app-constants.module';
import { User } from 'src/models/identity/user';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css'],
})
export class PostComponent implements OnInit {
  public user: User;
  public votesNumber: number;

  constructor() {}

  ngOnInit(): void {
    // Fetch data in post service
    this.user = new User(
      Guid.create(),
        'gosho_trapov',
        'Gosho',
        'Trapov',
        'gotra@bg.com',
        AppConstants.FALLBACK_PROFILE_ICON,
        new Array(),
        new Array()
    );

    this.votesNumber = 23;
  }
}
