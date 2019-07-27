import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from './_service/authentication.service';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
	isLoggedIn: boolean;

	constructor(private authenticationService: AuthenticationService) { }

	ngOnInit(): void {
		this.isLoggedIn = this.authenticationService.isLoggedIn();
	}
}
