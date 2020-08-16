import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonVariables } from '../_config/common-variables';
import { map } from 'rxjs/operators';
import { UserSubscriptionMapper } from '../_mappers/user-subscription-mapper';
import { Observable } from 'rxjs';
import { UserSubscription } from '../_models/user-subscription';
import { RoutingService } from './routing.service';
import { TimeSpanWrapper } from '../_models/time-span-wrapper';
import { TimeSpanWrapperMapper } from '../_mappers/time-span-wrapper-mapper';

@Injectable({
    providedIn: 'root'
})
export class SubscriptionsService {
    constructor(
        private routingService: RoutingService,
        private http: HttpClient) { }

    create(formData: FormData) {
        formData.append("applicationId", CommonVariables.ApplicationId);

        return this.http.post(this.routingService.getCreateSubscriptionUri(), formData);
    }

    getAll(userId: string): Observable<UserSubscription[]> {
        return this.http.get<UserSubscription[]>(this.routingService.getSubscriptionsUri() + userId).pipe(map(UserSubscriptionMapper.convertAll));
    }

    getSubscriptionRemainingTime(userId: string): Observable<TimeSpanWrapper> {
        return this.http.get<TimeSpanWrapper>(this.routingService.getSubscriptionRemainingTimeUri() + userId).pipe(map(TimeSpanWrapperMapper.convert));
    }
}
