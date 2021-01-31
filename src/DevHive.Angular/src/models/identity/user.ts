import { Guid } from 'guid-typescript';
import {Language} from '../language';
import {Technology} from '../technology';

export class User {
  private _id : Guid;
  private _lastName : string;
  private _firstName : string;
  private _userName : string;
  private _email: string;
  private _imageUrl : string;
  private _languages: Language[];
  private _technologies: Technology[];

  constructor(id: Guid, userName: string, firstName: string, lastName: string, email: string, imageUrl: string, languages: Language[], technologies: Technology[]) {
    this.id = id;
    this.userName = userName;
    this.firstName = firstName;
    this.lastName = lastName;
    this.email = email;
    this.imageUrl = imageUrl;
    this.technologies = technologies;
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

  public get imageUrl(): string {
    return this._imageUrl;
  }
  public set imageUrl(v: string) {
    this._imageUrl = v;
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
}
