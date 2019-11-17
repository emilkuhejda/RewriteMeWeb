import { Injectable } from '@angular/core';
import { RoutingService } from './routing.service';
import { HttpClient } from '@angular/common/http';
import { TimeSpanWrapper } from '../_models/time-span-wrapper';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { TimeSpanWrapperMapper } from '../_mappers/time-span-wrapper-mapper';

@Injectable({
    providedIn: 'root'
})
export class UserSubscriptionService {
    constructor(
        private routingService: RoutingService,
        private http: HttpClient) { }

    getSubscriptionRemainingTime(): Observable<TimeSpanWrapper> {
        return this.http.get<TimeSpanWrapper>(this.routingService.getSubscriptionRemainingTimeUri()).pipe(map(TimeSpanWrapperMapper.convert));
    }
}
