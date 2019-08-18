import { Injectable } from '@angular/core';
import { RoutingService } from './routing.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class UserSubscriptionService {
    constructor(
        private routingService: RoutingService,
        private http: HttpClient) { }

    getSubscriptionRemainingTimeUri(): Observable<string> {
        return this.http.get<string>(this.routingService.getSubscriptionRemainingTimeUri());
    }
}