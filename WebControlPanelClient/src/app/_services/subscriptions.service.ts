import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonVariables } from '../_config/common-variables';
import { map } from 'rxjs/operators';
import { UserSubscriptionMapper } from '../_mappers/user-subscription-mapper';
import { Observable } from 'rxjs';
import { UserSubscription } from '../_models/user-subscription';

@Injectable({
    providedIn: 'root'
})
export class SubscriptionsService {
    constructor(private http: HttpClient) { }

    create(formData: FormData) {
        formData.append("applicationId", CommonVariables.ApplicationId);

        return this.http.post(CommonVariables.ApiUrl + CommonVariables.ApiCreateSubscriptionPath, formData);
    }

    getAll(userId: string): Observable<UserSubscription[]> {
        return this.http.get<UserSubscription[]>(CommonVariables.ApiUrl + CommonVariables.ApiSubscriptionsPath + userId).pipe(map(UserSubscriptionMapper.convertAll));
    }
}
