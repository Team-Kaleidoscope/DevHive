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
import { CloudinaryService } from 'src/app/services/cloudinary.service';

@Component({
  selector: 'app-post-page',
  templateUrl: './post-page.component.html',
  styleUrls: ['./post-page.component.css']
})
export class PostPageComponent implements OnInit {
  private _title = 'Post';
  public dataArrived = false;
  public loggedIn = false;
  public editable = false;
  public editingPost = false;
  public postId: Guid;
  public post: Post;
  public files: File[];
  public editPostFormGroup: FormGroup;
  public addCommentFormGroup: FormGroup;

  constructor(private _titleService: Title, private _router: Router, private _fb: FormBuilder, private _tokenService: TokenService, private _postService: PostService, private _commentService: CommentService, private _cloudinaryService: CloudinaryService){
    this._titleService.setTitle(this._title);
  }

  ngOnInit(): void {
    this.loggedIn = this._tokenService.getTokenFromSessionStorage() !== '';
    this.postId = Guid.parse(this._router.url.substring(6));
    this.files = [];

    // Gets the post and the logged in user and compares them,
    // to determine if the current post is made by the user
    this._postService.getPostRequest(this.postId).subscribe(
      (result: object) => {
        this.post = result as Post;
        this.post.fileURLs = Object.values(result)[7];
        if (this.loggedIn) {
          this.editable = this.post.creatorUsername === this._tokenService.getUsernameFromSessionStorageToken();
        }
        if (this.post.fileURLs.length > 0) {
          this.loadFiles();
        }
        else {
          this.dataArrived = true;
        }
      },
      (err: HttpErrorResponse) => {
        this._router.navigate(['/not-found']);
      }
    );

    this.editPostFormGroup = this._fb.group({
      newPostMessage: new FormControl(''),
      fileUpload: new FormControl('')
    });

    this.addCommentFormGroup = this._fb.group({
      newComment: new FormControl('')
    });
  }

  private loadFiles(): void {
    for (const fileURL of this.post.fileURLs) {
      this._cloudinaryService.getFileRequest(fileURL).subscribe(
        (result: object) => {
          const file = result as File;
          const tmp = {
            name: fileURL.match('(?<=\/)(?:.(?!\/))+$')?.pop() ?? 'Attachment'
          };

          Object.assign(file, tmp);
          this.files.push(file);

          if (this.files.length === this.post.fileURLs.length) {
            this.dataArrived = true;
          }
        }
      );
    }
  }

  backToFeed(): void {
    this._router.navigate(['/']);
  }

  backToProfile(): void {
    this._router.navigate(['/profile/' + this.post.creatorUsername]);
  }

  toLogin(): void {
    this._router.navigate(['/login']);
  }

  onFileUpload(event: any): void {
    this.files.push(...event.target.files);
    this.editPostFormGroup.get('fileUpload')?.reset();
  }

  removeAttachment(fileName: string): void {
    this.files = this.files.filter(x => x.name !== fileName);
  }

  editPost(): void {
    if (this._tokenService.getTokenFromSessionStorage() === '') {
      this.toLogin();
      return;
    }

    if (this.editingPost) {
      let newMessage = this.editPostFormGroup.get('newPostMessage')?.value;
      if (newMessage === '') {
        newMessage = this.post.message;
      }
      this._postService.putPostWithSessionStorageRequest(this.postId, newMessage, this.files).subscribe(
        (result: object) => {
          this.reloadPage();
        }
      );
      this.dataArrived = false;
    }
    this.editingPost = !this.editingPost;
  }

  addComment(): void {
    if (!this.loggedIn) {
      this._router.navigate(['/login']);
      return;
    }

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
