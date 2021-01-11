import { Component, OnInit } from '@angular/core';
import { Guid } from 'guid-typescript';
import { User } from 'src/models/identity/user';

@Component({
	selector: 'app-post',
	templateUrl: './post.component.html',
	styleUrls: ['./post.component.css'],
})
export class PostComponent implements OnInit {
	public user: User;
	public votesNumber: number;

	constructor() {}

	ngOnInit(): void {
		//Fetch data in post service
		this.user = new User(
			Guid.create(),
			'gosho_trapov',
			'Gosho',
			'Trapov',
			'assets/images/feed/profile-pic.png'
		);

		this.votesNumber = 23;
	}
}
