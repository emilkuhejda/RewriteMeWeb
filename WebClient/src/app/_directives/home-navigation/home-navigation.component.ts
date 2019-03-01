import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
	selector: 'app-home-navigation',
	templateUrl: './home-navigation.component.html',
	styleUrls: ['./home-navigation.component.css']
})
export class HomeNavigationComponent implements OnInit {
	isHome: boolean = true;

	constructor(private router: Router) { }

	ngOnInit() {
		let parts = this.router.url.split("#");
		this.isHome = parts[0] === "/";

		if (parts[1]) {
			let element = document.querySelector("#" + parts[1]);
			element.scrollIntoView();
		}

		var htmlElement = document.getElementsByTagName('html')[0];
		if (this.isHome) {
			htmlElement.classList.add('full-height');
		} else {
			htmlElement.classList.remove('full-height');
		}
	}

	navigateToHome(fragment: string) {
		if (this.isHome) {
			let element = document.querySelector("#" + fragment);
			element.scrollIntoView();
		} else {
			this.router.navigate(['/'], { fragment: fragment });
		}
	}
}
