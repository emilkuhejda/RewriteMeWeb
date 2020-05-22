import { Component, OnInit, OnDestroy } from '@angular/core';
import { MsalService } from './_services/msal.service';
import { MessageCenterService } from './_services/message-center.service';
import { AlertService } from './_services/alert.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {
    private recognitionErrorMethod: string = "recognition-error";

    constructor(
        private msalService: MsalService,
        private messageCenterService: MessageCenterService,
        private alertService: AlertService) { }

    ngOnInit(): void {
        this.messageCenterService.startConnection();
        this.messageCenterService.addListener(this.recognitionErrorMethod, (fileName: string) => this.handleRecognitionErrorMethod(fileName, this.alertService));
    }

    private handleRecognitionErrorMethod(fileName: string, alertService) {
        alertService.error(`File '${fileName}' was not successfully transcribed. Please try it again or contact us.`);
    }

    ngOnDestroy(): void {
        this.messageCenterService.stopConnection();
    }

    logout(): void {
        this.msalService.logout();
    }
}
