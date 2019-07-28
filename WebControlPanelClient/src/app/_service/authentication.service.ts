import { Injectable, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonVariables } from '../_config/common-variables';
import { User } from '../_models/user';
import { map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class AuthenticationService {
    statusChanged: EventEmitter<boolean> = new EventEmitter();

    constructor(private http: HttpClient) { }

    login(username: string, password: string) {
        return this.http.post<any>(CommonVariables.ApiUrl + CommonVariables.ApiAuthenticatePath, { username: username, password: password })
            .pipe(map(user => {
                if (user && user.token) {
                    localStorage.setItem(CommonVariables.CurrentUser, JSON.stringify(user));
                }

                this.statusChanged.emit(this.isLoggedIn());

                return user;
            }));
    }

    logout() {
        localStorage.clear();
        this.statusChanged.emit(this.isLoggedIn());
    }

    getUser(): User {
        return JSON.parse(localStorage.getItem(CommonVariables.CurrentUser));
    }

    isLoggedIn(): boolean {
        let currentUser = this.getUser();
        return currentUser !== null && currentUser.token !== null;
    }
}
