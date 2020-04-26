import { Injectable } from '@angular/core';
import { RoutingService } from './routing.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class MailService {
    constructor(
        private routingService: RoutingService,
        private http: HttpClient) { }

    sendEmail(recipient: string, fileItemId: string): Observable<any> {
        return this.http.post(this.routingService.getMailUri(), { fileItemId: fileItemId, recipient: recipient });
    }
}
