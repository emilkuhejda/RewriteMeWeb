import { Injectable } from '@angular/core';
import { RoutingService } from './routing.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class UtilsService {
    constructor(
        private routingService: RoutingService,
        private http: HttpClient) { }

    generateHangfireAccess(): Observable<any> {
        return this.http.get(this.routingService.getGenerateHangfireAccessUri());
    }

    hasAccess(): Observable<boolean> {
        return this.http.get<boolean>(this.routingService.getHasAccessUri());
    }
}
