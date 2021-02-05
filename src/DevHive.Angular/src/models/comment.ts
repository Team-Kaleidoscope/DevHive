import { Guid } from 'guid-typescript';

export class Comment {
  private _commentId: Guid;
  private _postId: Guid;
  private _issuerFirstName: string;
  private _issuerLastName: string;
  private _issuerUsername: string;
  private _message: string;
  private _timeCreated: Date;

  constructor(commentId: Guid, postId: Guid, issuerFirstName: string, issuerLastName: string, issuerUsername: string, message: string, timeCreated: Date) {
    this.commentId = commentId;
    this.postId = postId;
    this.issuerFirstName = issuerFirstName;
    this.issuerLastName = issuerLastName;
    this.issuerUsername = issuerUsername;
    this.message = message;
    this.timeCreated = timeCreated;
  }

  public get commentId(): Guid {
    return this._commentId;
  }
  public set commentId(v: Guid) {
    this._commentId = v;
  }

  public get postId(): Guid {
    return this._postId;
  }
  public set postId(v: Guid) {
    this._postId = v;
  }

  public get issuerFirstName(): string {
    return this._issuerFirstName;
  }
  public set issuerFirstName(v: string) {
    this._issuerFirstName = v;
  }

  public get issuerLastName(): string {
    return this._issuerLastName;
  }
  public set issuerLastName(v: string) {
    this._issuerLastName = v;
  }

  public get issuerUsername(): string {
    return this._issuerUsername;
  }
  public set issuerUsername(v: string) {
    this._issuerUsername = v;
  }

  public get message(): string {
    return this._message;
  }
  public set message(v: string) {
    this._message = v;
  }

  public get timeCreated(): Date {
    return this._timeCreated;
  }
  public set timeCreated(v: Date) {
    this._timeCreated = v;
  }
}
