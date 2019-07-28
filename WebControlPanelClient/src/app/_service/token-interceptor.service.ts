import { Injectable } from '@angular/core';
import { AuthenticationService } from './authentication.service';
import { HttpRequest, HttpHandler } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})
export class TokenInterceptorService {
    constructor(private authenticationService: AuthenticationService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler) {
        let currentUser = this.authenticationService.getAdministrator();
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
