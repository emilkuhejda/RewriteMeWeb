import { Injectable } from '@angular/core';
import * as signalR from "@aspnet/signalr";
import { RoutingService } from './routing.service';
import { MsalService } from './msal.service';
import { CacheItem } from '../_models/cache-item';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class CachService {
    private hubConnection: signalR.HubConnection;

    constructor(
        private routingService: RoutingService,
        private msalService: MsalService,
        private http: HttpClient) { }

    startConnection() {
        this.hubConnection = new signalR.HubConnectionBuilder().withUrl(this.routingService.getCacheHubUri()).build();
        this.hubConnection.start();
    }

    addListener(method, action) {
        let identity = this.msalService.getIdentity();
        this.hubConnection.on(`${method}-${identity.id}`, action);
    }

    getCacheItem(fileItemId: string): Observable<CacheItem> {
        return this.http.get<CacheItem>(this.routingService.getCacheItemUri() + fileItemId);
    }
}
