import * as signalR from "@aspnet/signalr";
import { Injectable } from '@angular/core';
import { RoutingService } from './routing.service';
import { MsalService } from './msal.service';

@Injectable({
    providedIn: 'root'
})
export class MessageCenterService {
    private hubConnection: signalR.HubConnection;

    constructor(
        private routingService: RoutingService,
        private msalService: MsalService) { }

    startConnection() {
        this.hubConnection = new signalR.HubConnectionBuilder().withUrl(this.routingService.getMessageHubUri()).build();
        this.hubConnection.start();
    }

    stopConnection() {
        this.hubConnection.stop();
    }

    addListener(method: string, action: any) {
        let identity = this.msalService.getIdentity();
        this.hubConnection.on(`${method}-${identity.id}`, action);
    }
}
