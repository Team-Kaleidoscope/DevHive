import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Guid } from 'guid-typescript';
import { CommentService } from 'src/app/services/comment.service';
import { UserService } from 'src/app/services/user.service';
import { Comment } from 'src/models/comment';
import { User } from 'src/models/identity/user';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.css']
})
export class CommentComponent implements OnInit {
  public loaded = false;
  public user: User;
  public comment: Comment;
  public timeCreated: string;
  @Input() paramId: string;

  constructor(private _router: Router, private _commentService: CommentService, private _userService: UserService)
  { }

  ngOnInit(): void {
    this.comment = this._commentService.getDefaultComment();
    this.user = this._userService.getDefaultUser();

    this._commentService.getCommentRequest(Guid.parse(this.paramId)).subscribe(
      (result: object) => {
        Object.assign(this.comment, result);

        this.timeCreated = new Date(this.comment.timeCreated).toLocaleString('en-GB');
        this.loadUser();
      }
    );
  }

  private loadUser(): void {
    this._userService.getUserByUsernameRequest(this.comment.issuerUsername).subscribe(
      (result: object) => {
        Object.assign(this.user, result);
        this.loaded = true;
      }
    );
  }

  goToAuthorProfile(): void {
    this._router.navigate(['/profile/' + this.comment.issuerUsername]);
  }

  goToCommentPage(): void {
    this._router.navigate(['/comment/' + this.comment.commentId]);
  }
}
