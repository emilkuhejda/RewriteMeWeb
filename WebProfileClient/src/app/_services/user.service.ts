import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RoutingService } from './routing.service';
import { Observable } from 'rxjs';
import { Identity } from '../_models/identity';

@Injectable({
    providedIn: 'root'
})
export class UserService {
    constructor(
        private routingService: RoutingService,
        private http: HttpClient) { }

    updateUser(userData): Observable<Identity> {
        return this.http.put<Identity>(this.routingService.getUpdateUserUri(), userData);
    }
}
