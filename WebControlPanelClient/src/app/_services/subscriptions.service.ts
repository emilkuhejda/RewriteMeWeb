import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonVariables } from '../_config/common-variables';

@Injectable({
    providedIn: 'root'
})
export class SubscriptionsService {
    constructor(private http: HttpClient) { }

    getAll(userId: string) {
        return this.http.get(CommonVariables.ApiUrl + CommonVariables.ApiSubscriptionsPath + userId);
    }
}
