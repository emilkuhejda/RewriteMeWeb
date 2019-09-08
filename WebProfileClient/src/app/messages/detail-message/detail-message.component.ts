import { Component, OnInit } from '@angular/core';
import { InformationMessageService } from 'src/app/_services/information-message.service';
import { InformationMessage } from 'src/app/_models/information-message';
import { AlertService } from 'src/app/_services/alert.service';
import { ActivatedRoute } from '@angular/router';
import { ErrorResponse } from 'src/app/_models/error-response';

@Component({
    selector: 'app-detail-message',
    templateUrl: './detail-message.component.html',
    styleUrls: ['./detail-message.component.css']
})
export class DetailMessageComponent implements OnInit {
    informationMessage: InformationMessage;

    constructor(
        private route: ActivatedRoute,
        private informationMessageService: InformationMessageService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.route.paramMap.subscribe(paramMap => {
            var messageId = paramMap.get('messageId');
            this.informationMessageService.get(messageId).subscribe(
                (informationMessage: InformationMessage) => {
                    this.informationMessage = informationMessage;
                    if (informationMessage.isUserSpecific) {
                        if (!informationMessage.wasOpened) {
                            this.informationMessageService.markAsOpened(messageId).subscribe(
                                (informationMessage: InformationMessage) => {
                                    this.informationMessage.wasOpened = informationMessage.wasOpened;
                                }
                            );
                        }
                    }
                    else {
                        this.informationMessageService.markAsOpenedLocally(informationMessage.id);
                    }
                },
                (err: ErrorResponse) => {
                    this.alertService.error(err.message);
                });
        });
    }
}
