import { Component, OnInit, OnDestroy } from '@angular/core';
import { MsalService } from './_services/msal.service';
import { MessageCenterService } from './_services/message-center.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {
    constructor(
        private msalService: MsalService,
        private messageCenterService: MessageCenterService) { }

    ngOnInit(): void {
        this.messageCenterService.startConnection();
    }

    ngOnDestroy(): void {
        this.messageCenterService.stopConnection();
    }

    logout(): void {
        this.msalService.logout();
    }
}
