import { Injectable } from '@angular/core';
import { RoutingService } from './routing.service';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})
export class SettingsService {
    constructor(
        private routingService: RoutingService,
        private http: HttpClient) { }

    changeStorage(): Observable<any> {
        return this.http.get<any>(this.routingService.getChangeStorageUri());
    }

    cleanUp(): Observable<any> {
        return this.http.put<any>(this.routingService.getCleanUpUri(), {});
    }
}
