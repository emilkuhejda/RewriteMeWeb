import { Injectable } from '@angular/core';
import { CommonVariables } from '../_config/common-variables';
import { map } from 'rxjs/operators';
import { User } from '../_models/user';
import { HttpClient } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})
export class AuthenticationService {
    constructor(private http: HttpClient) { }

    login(username: string, password: string) {
        return this.http.post<any>(CommonVariables.ApiUrl + CommonVariables.ApiAuthenticatePath, { username: username, password: password })
            .pipe(map(user => {
                if (user && user.token) {
                    localStorage.setItem(CommonVariables.CurrentUser, JSON.stringify(user));
                }

                return user;
            }));
    }

    logout() {
        localStorage.clear();
    }

    getUser(): User {
        return JSON.parse(localStorage.getItem(CommonVariables.CurrentUser));
    }
}
