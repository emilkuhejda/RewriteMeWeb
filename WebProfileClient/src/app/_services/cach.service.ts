import { Injectable } from '@angular/core';
import * as signalR from "@aspnet/signalr";
import { RoutingService } from './routing.service';
import { MsalService } from './msal.service';
import { CacheItem } from '../_models/cache-item';

@Injectable({
    providedIn: 'root'
})
export class CachService {
    private hubConnection: signalR.HubConnection;

    constructor(
        private routingService: RoutingService,
        private msalService: MsalService) { }

    public startConnection() {
        this.hubConnection = new signalR.HubConnectionBuilder().withUrl(this.routingService.getCacheUri()).build();
        this.hubConnection.start();
    }

    public addListener(action) {
        let identity = this.msalService.getIdentity();
        this.hubConnection.on(identity.id, (cacheItem: CacheItem) => {
            action(cacheItem);
        });
    }
}
