import { Guid } from "guid-typescript";

export class User {
	private id: Guid;
	private userName: string;
	private firstName: string;
	private lastName: string;

	constructor(id: Guid, userName: string, firstName: string, lastName: string) {
		this.id = id;
		this.userName = userName;
		this.firstName = firstName;
		this.lastName = lastName;
	}
}
