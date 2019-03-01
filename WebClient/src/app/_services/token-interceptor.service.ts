import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler } from '@angular/common/http';
import { AuthenticationService } from './authentication.service';

@Injectable({
	providedIn: 'root'
})
export class TokenInterceptorService {
	constructor(private authenticationService: AuthenticationService) { }

	intercept(request: HttpRequest<any>, next: HttpHandler) {
		let currentUser = this.authenticationService.getUser();
		if (currentUser && currentUser.token) {
			request = request.clone({
				setHeaders: {
					Authorization: "Bearer " + currentUser.token
				}
			});
		}

		return next.handle(request);
	}
}
