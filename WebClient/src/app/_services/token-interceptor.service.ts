import { Injectable } from '@angular/core';
import { MsalService } from './msal.service';
import { HttpHandler, HttpRequest } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})
export class TokenInterceptorService {
    constructor(private msalService: MsalService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler) {
        if (this.msalService.isLoggedIn()) {
            let token = this.msalService.getToken();
            if (token) {
                request = request.clone({
                    setHeaders: {
                        Authorization: "Bearer " + token
                    }
                });
            }
        }

        return next.handle(request);
    }
}
