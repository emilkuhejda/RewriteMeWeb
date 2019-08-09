import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RoutingService } from './routing.service';

@Injectable({
    providedIn: 'root'
})
export class ContactFormService {
    constructor(
        private routingService: RoutingService,
        private http: HttpClient) { }

    create(data) {
        return this.http.post(this.routingService.getCreateContactFormUri(), data);
    }
}
