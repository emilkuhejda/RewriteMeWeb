import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../_models/user';
import { UserMapper } from '../_mappers/user-mapper';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { RoutingService } from './routing.service';

@Injectable({
    providedIn: 'root'
})
export class UserService {
    constructor(
        private routingService: RoutingService,
        private http: HttpClient) { }

    getAll(): Observable<User[]> {
        return this.http.get<User[]>(this.routingService.getUsersUri()).pipe(map(UserMapper.convertAll));
    }
}
