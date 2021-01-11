import { Guid } from "guid-typescript";

export class User {
	private _id : Guid;
	private _lastName : string;
	private _firstName : string;
	private _userName : string;
	private _imageUrl : string;

	constructor(id: Guid, userName: string, firstName: string, lastName: string, imageUrl: string) {
		this.id = id;
		this.userName = userName;
		this.firstName = firstName;
		this.lastName = lastName;
	}

	
	public get id() : Guid {
		return this._id;
	}
	public set id(v : Guid) {
		this._id = v;
	}
	
	public get userName() : string {
		return this._userName;
	}
	public set userName(v : string) {
		this._userName = v;
	}
	
	public get firstName() : string {
		return this._firstName;
	}
	public set firstName(v : string) {
		this._firstName = v;
	}
	
	public get lastName() : string {
		return this._lastName;
	}
	public set lastName(v: string) {
		this._lastName = v;
	}
	
	public get imageUrl() : string {
		return this._imageUrl;
	}
	public set imageUrl(v : string) {
		this._imageUrl = v;
	}
}
