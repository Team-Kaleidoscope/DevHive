import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { User } from 'src/models/identity/user';
import { PostComponent } from '../post/post.component';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.css']
})
export class FeedComponent implements OnInit {
  private _title = 'Feed';
  private _timeoutFetchData = 500;
  public dataArrived = false;
  public user: User;
  public posts: PostComponent[];

  constructor(private _titleService: Title, private _router: Router, private _userService: UserService) {
    this._titleService.setTitle(this._title);
  }

  ngOnInit(): void {
    this.posts = [
      new PostComponent(),
      new PostComponent(),
      new PostComponent(),
      new PostComponent(),
    ];

    if (sessionStorage.getItem('UserCred')) {
      // Workaround for waiting the fetch response
      // TODO: properly wait for it, before loading the page contents
      setTimeout(() =>
                 {
                   this.user = this._userService.fetchUserFromSessionStorage();
                 },
                 this._timeoutFetchData);
      setTimeout(() =>
                 {
                   this.dataArrived = true;
                 },
                 this._timeoutFetchData + 100);
    } else {
      this._router.navigate(['/login']);
    }
  }
}
