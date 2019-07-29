import { Injectable } from '@angular/core';
import { AuthenticationService } from './authentication.service';
import { HttpRequest, HttpHandler } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})
export class TokenInterceptorService {
    constructor(private authenticationService: AuthenticationService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler) {
        let identity = this.authenticationService.getIdentity();
        if (identity && identity.token) {
            request = request.clone({
                setHeaders: {
                    Authorization: "Bearer " + identity.token
                }
            });
        }

        return next.handle(request);
    }
}
