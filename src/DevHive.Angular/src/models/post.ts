import { Guid } from 'guid-typescript';
import {Comment} from './comment';

export class Post {
  private _postId: Guid;
  private _creatorFirstName: string;
  private _creatorLastName: string;
  private _creatorUsername: string;
  private _message: string;
  private _timeCreated: Date;
  private _comments: Comment[];
  // _files

  constructor(postId: Guid, creatorFirstName: string, creatorLastName: string, creatorUsername: string, message: string, timeCreated: Date, comments: Comment[]) {
    this.postId = postId;
    this.creatorFirstName = creatorFirstName;
    this.creatorLastName = creatorLastName;
    this.creatorUsername = creatorUsername;
    this.message = message;
    this.timeCreated = timeCreated;
    this.comments = comments;
  }

  public get postId(): Guid {
    return this._postId;
  }
  public set postId(v: Guid) {
    this._postId = v;
  }

  public get creatorFirstName(): string {
    return this._creatorFirstName;
  }
  public set creatorFirstName(v: string) {
    this._creatorFirstName = v;
  }

  public get creatorLastName(): string {
    return this._creatorLastName;
  }
  public set creatorLastName(v: string) {
    this._creatorLastName = v;
  }

  public get creatorUsername(): string {
    return this._creatorUsername;
  }
  public set creatorUsername(v: string) {
    this._creatorUsername = v;
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

  public get comments(): Comment[] {
    return this._comments;
  }
  public set comments(v: Comment[]) {
    this._comments = v;
  }
}
