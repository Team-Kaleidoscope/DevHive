import {Guid} from 'guid-typescript';

export class Post {
  private _postId: Guid;
  // _creatorId
  private _message: string;
  private _timeCreated: Date;
  // _comments
  // _files

  constructor(postId: Guid, message: string, timeCreated: Date) {
    this.postId = postId;
    this.message = message;
    this.timeCreated = timeCreated;
  }

  public get postId(): Guid {
    return this._postId;
  }
  public set postId(v: Guid) {
    this._postId = v;
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
