import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from './_service/authentication.service';
import { Router } from '@angular/router';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
	isLoggedIn: boolean;

	constructor(
		private router: Router,
		private authenticationService: AuthenticationService) { }

	ngOnInit(): void {
		this.isLoggedIn = this.authenticationService.isLoggedIn();

		this.authenticationService.statusChanged.subscribe(() => {
			this.isLoggedIn = this.authenticationService.isLoggedIn();
		});
	}

	logout() {
		this.authenticationService.logout();
		window.location.reload();
	}
}
