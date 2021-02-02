import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { Guid } from 'guid-typescript';
import { CommentService } from 'src/app/services/comment.service';
import { PostService } from 'src/app/services/post.service';
import { TokenService } from 'src/app/services/token.service';
import { Post } from 'src/models/post';

@Component({
  selector: 'app-post-page',
  templateUrl: './post-page.component.html',
  styleUrls: ['./post-page.component.css']
})
export class PostPageComponent implements OnInit {
  private _title = 'Post';
  public editable = false;
  public editingPost = false;
  public postId: Guid;
  public post: Post;
  public editPostFormGroup: FormGroup;
  public addCommentFormGroup: FormGroup;

  constructor(private _titleService: Title, private _router: Router, private _fb: FormBuilder, private _tokenService: TokenService, private _postService: PostService, private _commentService: CommentService){
    this._titleService.setTitle(this._title);
  }

  ngOnInit(): void {
    this.postId = Guid.parse(this._router.url.substring(6));

    // Gets the post and the logged in user and compares them,
    // to determine if the current post is made by the user
    this._postService.getPostRequest(this.postId).subscribe(
      (result: object) => {
        this.post = result as Post;
        this.post.comments = this.post.comments.sort((x, y) => {
          return Date.parse(y.timeCreated.toString()) - Date.parse(x.timeCreated.toString());
        });
        this.editable = this.post.creatorUsername === this._tokenService.getUsernameFromSessionStorageToken();
      },
      (err: HttpErrorResponse) => {
        this._router.navigate(['/not-found']);
      }
    );

    this.editPostFormGroup = this._fb.group({
      newPostMessage: new FormControl('')
    });

    this.addCommentFormGroup = this._fb.group({
      newComment: new FormControl('')
    });
  }

  backToFeed(): void {
    this._router.navigate(['/']);
  }

  backToProfile(): void {
    this._router.navigate(['/profile/' + this.post.creatorUsername]);
  }

  editPost(): void {
    if (this._tokenService.getTokenFromSessionStorage() === '') {
      this._router.navigate(['/login']);
      return;
    }

    if (this.editingPost) {
      const newMessage = this.editPostFormGroup.get('newPostMessage')?.value;
      if (newMessage !== '') {
        this._postService.putPostWithSessionStorageRequest(this.postId, newMessage).subscribe(
          (result: object) => {
            this.reloadPage();
          }
        );
      }
    }
    this.editingPost = !this.editingPost;
  }

  addComment(): void {
    const newComment = this.addCommentFormGroup.get('newComment')?.value;
    if (newComment !== '' && newComment !== null) {
      this._commentService.createCommentWithSessionStorageRequest(this.postId, newComment).subscribe(
        (result: object) => {
          this.editPostFormGroup.reset();
          this.reloadPage();
        }
      );
    }
  }

  deletePost(): void {
    this._postService.deletePostWithSessionStorage(this.postId).subscribe(
      (result: object) => {
        this._router.navigate(['/profile/' + this._tokenService.getUsernameFromSessionStorageToken()]);
      }
    );
  }

  private reloadPage(): void {
    this._router.routeReuseStrategy.shouldReuseRoute = () => false;
    this._router.onSameUrlNavigation = 'reload';
    this._router.navigate([this._router.url]);
  }
}
