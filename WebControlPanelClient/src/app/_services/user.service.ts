import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonVariables } from '../_config/common-variables';
import { User } from '../_models/user';
import { UserMapper } from '../_mappers/user-mapper';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class UserService {
    constructor(private http: HttpClient) { }

    getAll(): Observable<User[]> {
        return this.http.get<User[]>(CommonVariables.ApiUrl + CommonVariables.ApiUsersPath).pipe(map(UserMapper.convertAll));
    }
}
