import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler } from '@angular/common/http';
import { MsalService } from './msal.service';

@Injectable({
    providedIn: 'root'
})
export class TokenInterceptorService {
    constructor(private msalService: MsalService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler) {
        let token = this.msalService.getToken();
        if (token) {
            request = request.clone({
                setHeaders: {
                    Authorization: "Bearer " + token
                }
            });
        }

        return next.handle(request);
    }
}
