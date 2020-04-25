import { Component, OnInit } from '@angular/core';
import { MsalService } from './_services/msal.service';
import { MessageCenterService } from './_services/message-center.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
    constructor(
        private messageCenterService: MessageCenterService,
        private msalService: MsalService) { }

    ngOnInit(): void {
        this.messageCenterService.startConnection();
    }

    logout(): void {
        this.msalService.logout();
    }
}
