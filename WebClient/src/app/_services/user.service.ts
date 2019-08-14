import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RoutingService } from './routing.service';
import { Observable } from 'rxjs';
import { UserRegistration } from '../_models/user-registration';

@Injectable({
    providedIn: 'root'
})
export class UserService {
    constructor(
        private routingService: RoutingService,
        private http: HttpClient) { }

    register(data): Observable<UserRegistration> {
        return this.http.post<UserRegistration>(this.routingService.getRegisterUserUri(), data);
    }
}
