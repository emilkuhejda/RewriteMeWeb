import { Component, OnInit } from '@angular/core';
import { InformationMessageService } from '../_services/information-message.service';
import { AlertService } from '../_services/alert.service';
import { InformationMessage } from '../_models/information-message';
import { ErrorResponse } from '../_models/error-response';

@Component({
    selector: 'app-messages',
    templateUrl: './messages.component.html',
    styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
    informationMessages: InformationMessage[];

    constructor(
        private informationMessageService: InformationMessageService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.informationMessageService.getAll(undefined).subscribe(
            (informationMessages: InformationMessage[]) => {
                informationMessages = informationMessages.sort((a, b) => {
                    return <any>new Date(b.datePublished) - <any>new Date(a.datePublished);
                });

                this.informationMessageService.updateWasOpenedProperty(informationMessages);
                this.informationMessages = informationMessages;
            },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
            });
    }
}
