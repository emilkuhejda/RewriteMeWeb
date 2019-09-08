import { Component, OnInit } from '@angular/core';
import { MsalService } from 'src/app/_services/msal.service';
import { InformationMessageService } from 'src/app/_services/information-message.service';
import { InformationMessage } from 'src/app/_models/information-message';

@Component({
    selector: 'app-topbar',
    templateUrl: './topbar.component.html',
    styleUrls: ['./topbar.component.css']
})
export class TopbarComponent implements OnInit {
    private recordsToDisplay: number = 3;

    userName: string;
    informationMessages: InformationMessage[];
    unopenedMessagesCount: number;

    constructor(
        private msalService: MsalService,
        private informationMessageService: InformationMessageService) { }

    ngOnInit() {
        this.msalService.identityChanged.subscribe(() => {
            this.initializeUserName();
        });

        this.informationMessageService.messageWasOpened.subscribe(() => {
            this.initializeMessages();
        });

        this.initializeUserName();
        this.initializeMessages();
    }

    private initializeUserName(): void {
        this.userName = this.msalService.getIdentityUserName();
    }

    private initializeMessages() {
        this.informationMessageService.getAll(this.recordsToDisplay).subscribe(
            (informationMessages: InformationMessage[]) => {
                informationMessages = informationMessages.sort((a, b) => {
                    return <any>new Date(b.datePublished) - <any>new Date(a.datePublished);
                });

                this.informationMessageService.updateWasOpenedProperty(informationMessages);
                this.unopenedMessagesCount = informationMessages.filter(x => !x.wasOpened).length;
                this.informationMessages = informationMessages.slice(0, 5);
            });
    }
}
