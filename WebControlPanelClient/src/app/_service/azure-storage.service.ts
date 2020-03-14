import { Injectable } from '@angular/core';
import { RoutingService } from '../_services/routing.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class AzureStorageService {
    constructor(
        private routingService: RoutingService,
        private http: HttpClient) { }

    migrate(): Observable<any> {
        return this.http.patch<any>(this.routingService.getMigrateUri(), {});
    }
}
