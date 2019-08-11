import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RoutingService } from './routing.service';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class UserService {
    constructor(
        private routingService: RoutingService,
        private http: HttpClient) { }

    updateUser(userData): Observable<any> {
        return this.http.put(this.routingService.getUpdateUserUri(), userData);
    }
}
