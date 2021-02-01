import {HttpErrorResponse} from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormControl, FormGroup} from '@angular/forms';
import { Router } from '@angular/router';
import { Guid } from 'guid-typescript';
import {PostService} from 'src/app/services/post.service';
import {TokenService} from 'src/app/services/token.service';
import {Post} from 'src/models/post';

@Component({
  selector: 'app-post-page',
  templateUrl: './post-page.component.html',
  styleUrls: ['./post-page.component.css']
})
export class PostPageComponent implements OnInit {
  public editable = false;
  public editingPost = false;
  public postId: Guid;
  public editPostFormGroup: FormGroup;

  constructor(private _router: Router, private _fb: FormBuilder, private _tokenService: TokenService, private _postService: PostService)
  { }

  ngOnInit(): void {
    this.postId = Guid.parse(this._router.url.substring(6));

    // Gets the post and the logged in user and compares them,
    // to determine if the current post is made by the user
    this._postService.getPostRequest(this.postId).subscribe(
      (result: object) => {
        const post = result as Post;
        this.editable = post.creatorUsername === this._tokenService.getUsernameFromSessionStorageToken();
      },
      (err: HttpErrorResponse) => {
        this._router.navigate(['/not-found']);
      }
    );

    this.editPostFormGroup = this._fb.group({
      newPostMessage: new FormControl('')
    });
  }

  backToFeed(): void {
    this._router.navigate(['/']);
  }

  editPost(): void {
    if (this.editingPost) {
      const newMessage = this.editPostFormGroup.get('newPostMessage')?.value;
      if (newMessage !== '') {
        this._postService.putPostWithSessionStorageRequest(this.postId, newMessage).subscribe(
          (result: object) => {
            // Reload the page
            this._router.routeReuseStrategy.shouldReuseRoute = () => false;
            this._router.onSameUrlNavigation = 'reload';
            this._router.navigate([this._router.url]);
          }
        );
      }
    }
    this.editingPost = !this.editingPost;
  }

  deletePost(): void {
    this._postService.deletePostWithSessionStorage(this.postId).subscribe(
      (result: object) => {
        this._router.navigate(['/profile/' + this._tokenService.getUsernameFromSessionStorageToken()]);
      }
    );
  }
}
