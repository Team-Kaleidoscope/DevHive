import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { Guid } from 'guid-typescript';
import { CommentService } from 'src/app/services/comment.service';
import { TokenService } from 'src/app/services/token.service';
import { Comment } from 'src/models/comment';

@Component({
  selector: 'app-comment-page',
  templateUrl: './comment-page.component.html',
  styleUrls: ['./comment-page.component.css']
})
export class CommentPageComponent implements OnInit {
  private _title = 'Comment';
  public loaded = false;
  public loggedIn = false;
  public editable = false;
  public editingComment = false;
  public commentId: Guid;
  public comment: Comment;
  public editCommentFormGroup: FormGroup;

  constructor(private _titleService: Title, private _router: Router, private _fb: FormBuilder, private _tokenService: TokenService, private _commentService: CommentService){
    this._titleService.setTitle(this._title);
  }

  ngOnInit(): void {
    this.loggedIn = this._tokenService.getTokenFromSessionStorage() !== '';
    this.commentId = Guid.parse(this._router.url.substring(9));

    // Gets the post and the logged in user and compares them,
    // to determine if the current post is made by the user
    this._commentService.getCommentRequest(this.commentId).subscribe(
      (result: object) => {
        this.comment = result as Comment;
        if (this.loggedIn) {
          this.editable = this.comment.issuerUsername === this._tokenService.getUsernameFromSessionStorageToken();
        }
        this.loaded = true;
      },
      (err: HttpErrorResponse) => {
        this._router.navigate(['/not-found']);
      }
    );

    this.editCommentFormGroup = this._fb.group({
      newCommentMessage: new FormControl('')
    });
  }

  toPost(): void {
    this._router.navigate(['/post/' + this.comment.postId]);
  }

  editComment(): void {
    if (this._tokenService.getTokenFromSessionStorage() === '') {
      this._router.navigate(['/login']);
      return;
    }

    if (this.editingComment) {
      const newMessage = this.editCommentFormGroup.get('newCommentMessage')?.value;
      if (newMessage !== '') {
        console.log(this.commentId);
        this._commentService.putCommentWithSessionStorageRequest(this.commentId, this.comment.postId, newMessage).subscribe(
          (result: object) => {
            this.reloadPage();
          }
        );
      }
    }
    this.editingComment = !this.editingComment;
  }

  deleteComment(): void {
    this._commentService.deleteCommentWithSessionStorage(this.commentId).subscribe(
      (result: object) => {
        this.toPost();
      }
    );
  }

  private reloadPage(): void {
    this._router.routeReuseStrategy.shouldReuseRoute = () => false;
    this._router.onSameUrlNavigation = 'reload';
    this._router.navigate([this._router.url]);
  }
}
