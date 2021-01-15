import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { Guid } from 'guid-typescript';
import jwt_decode from 'jwt-decode';
import { FeedService } from 'src/app/services/feed.service';
import { User } from 'src/models/identity/user';
import { IJWTPayload } from 'src/interfaces/jwt-payload';
import { IUserCredentials } from 'src/interfaces/user-credentials';
import { PostComponent } from '../post/post.component';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.css']
})
export class FeedComponent implements OnInit {
  private _title = 'Feed';
  public user: User;
  public posts: PostComponent[];

  constructor(private titleService: Title, private service: FeedService, private router: Router) {
    this.titleService.setTitle(this._title);
  }

  ngOnInit(): void {
    // Default values, so it doesn't give an error while initializing
    this.user = new User(Guid.createEmpty(), '', '', '', '');

    this.posts = [
      new PostComponent(),
      new PostComponent(),
      new PostComponent(),
      new PostComponent(),
    ];

    if (sessionStorage.getItem('UserCred')) {
      const jwt: IJWTPayload = JSON.parse(sessionStorage.getItem('UserCred') ?? '');
      const userCred = jwt_decode<IUserCredentials>(jwt.token);
      this.saveUserData(userCred.ID, jwt.token);
    } else {
      this.router.navigate(['/login']);
    }
  }

  saveUserData(userId: Guid, authToken: string): void {
    fetch(`http://localhost:5000/api/User?Id=${userId}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + authToken
      }
    }).then(response => response.json()).then(data => Object.assign(this.user, data));
  }
}
