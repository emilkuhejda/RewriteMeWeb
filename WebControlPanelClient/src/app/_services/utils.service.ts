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

    hasAccess(): Observable<boolean> {
        return this.http.get<boolean>(this.routingService.getHasAccessUri());
    }

    isDeploymentSuccessful(): Observable<any> {
        return this.http.get<any>(this.routingService.getIsDeploymentSuccessfulUri());
    }

    generateHangfireAccess(): Observable<any> {
        return this.http.get(this.routingService.getGenerateHangfireAccessUri());
    }

    resetDatabase(data: any): Observable<any> {
        return this.http.put<any>(this.routingService.getResetDatabaseUri(), data);
    }

    deleteDatabase(data: any): Observable<any> {
        return this.http.put<any>(this.routingService.getDeleteDatabaseUri(), data);
    }
}
