import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RoutingService } from './routing.service';

@Injectable({
    providedIn: 'root'
})
export class UserService {
    constructor(
        private routingService: RoutingService,
        private http: HttpClient) { }

    register(data) {
        return this.http.post(this.routingService.getRegisterUserUri(), data);
    }
}
