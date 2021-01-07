export class RegisterUser
{
	private _userName: string;
	private _password: string;

	// constructor(userName: string, password: string)
	// {
	// 	this.userName = userName;
	// 	this.password = password;
	// }

	public get userName()
	{
		return this._userName;
	}

	public set userName(userName: string)
	{
		if (userName.length <= 3)
			throw new Error('Username cannot be less than 3 characters long!');

		this._userName = userName;
	}

	public get password()
	{
		return this._password;
	}

	public set password(pass: string)
	{
		if (pass.length <= 5)
			throw Error("Password too short!");
		
		this._password = pass;
	}
}
