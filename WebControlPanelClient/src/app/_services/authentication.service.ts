import { Injectable, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonVariables } from '../_config/common-variables';
import { map } from 'rxjs/operators';
import { Identity } from '../_models/identity';

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

    getIdentity(): Identity {
        return JSON.parse(localStorage.getItem(CommonVariables.CurrentUser));
    }

    isLoggedIn(): boolean {
        let identity = this.getIdentity();
        return identity !== null && identity.token !== null;
    }
}
