import { Guid } from 'guid-typescript';
import { Language } from '../language';
import { Technology } from '../technology';
import { Friend } from './friend';
import { Role } from './role';

export class User {
  private _id : Guid;
  private _lastName : string;
  private _firstName : string;
  private _userName : string;
  private _email: string;
  private _profilePictureURL : string;
  private _languages: Language[];
  private _technologies: Technology[];
  private _roles: Role[];
  private _friends: Friend[];

  constructor(id: Guid, userName: string, firstName: string, lastName: string, email: string, profilePictureURL: string, languages: Language[], technologies: Technology[], roles: Role[], friends: Friend[]) {
    this.id = id;
    this.userName = userName;
    this.firstName = firstName;
    this.lastName = lastName;
    this.email = email;
    this._profilePictureURL = profilePictureURL;
    this.languages = languages;
    this.technologies = technologies;
    this.roles = roles;
  }

  public get id(): Guid {
    return this._id;
  }
  public set id(v: Guid) {
    this._id = v;
  }

  public get userName(): string {
    return this._userName;
  }
  public set userName(v: string) {
    this._userName = v;
  }

  public get firstName(): string {
    return this._firstName;
  }
  public set firstName(v: string) {
    this._firstName = v;
  }

  public get lastName(): string {
    return this._lastName;
  }
  public set lastName(v: string) {
    this._lastName = v;
  }

  public get email(): string {
    return this._email;
  }
  public set email(v: string) {
    this._email = v;
  }

  public get profilePictureURL(): string {
    return this._profilePictureURL;
  }
  public set profilePictureURL(v: string) {
    this._profilePictureURL = v;
  }

  public get languages(): Language[] {
    return this._languages;
  }
  public set languages(v: Language[]) {
    this._languages = v;
  }

  public get technologies(): Technology[] {
    return this._technologies;
  }
  public set technologies(v: Technology[]) {
    this._technologies = v;
  }

  public get roles(): Role[] {
    return this._roles;
  }
  public set roles(v: Role[]) {
    this._roles = v;
  }

  public get friends(): Friend[] {
    return this._friends;
  }
  public set friends(v: Friend[]) {
    this._friends = v;
  }
}
