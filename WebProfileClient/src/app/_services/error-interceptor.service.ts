import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ErrorResponse } from '../_models/error-response';
import { MsalService } from './msal.service';

@Injectable({
    providedIn: 'root'
})
export class ErrorInterceptorService {
    constructor(private msalService: MsalService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request).pipe(
            catchError(err => {
                if (err.status === 401 || err.status === 403) {
                    this.msalService.logout();
                    location.reload(true);
                }

                let errorResponse = new ErrorResponse(err);
                return throwError(errorResponse);
            })
        );
    }
}
