import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { AuthenticationService } from './authentication.service';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ErrorResponse } from '../_models/error-response';

@Injectable({
    providedIn: 'root'
})
export class ErrorInterceptorService {
    constructor(private authenticationService: AuthenticationService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request).pipe(
            catchError(err => {
                if (err.status === 401) {
                    this.authenticationService.logout();
                    location.reload(true);
                }
                
                let errorResponse = new ErrorResponse(err);
                return throwError(errorResponse);
            })
        );
    }
}
