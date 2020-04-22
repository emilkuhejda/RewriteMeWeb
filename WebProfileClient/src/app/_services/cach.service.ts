import { Injectable } from '@angular/core';
import * as signalR from "@aspnet/signalr";
import { RoutingService } from './routing.service';

@Injectable({
    providedIn: 'root'
})
export class CachService {
    private hubConnection: signalR.HubConnection;

    constructor(private routingService: RoutingService) { }

    public startConnection() {
        this.hubConnection = new signalR.HubConnectionBuilder().withUrl(this.routingService.getCacheUri()).build();

        this.hubConnection.start()
            .then(() => {
                console.log("Start connection");
            })
            .catch((err) => {
                console.log(err);
            });
    }

    public addListener() {
        this.hubConnection.on('transferdata', data => {
            console.log(data);
        });
    }
}
