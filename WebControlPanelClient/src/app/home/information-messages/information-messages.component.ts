import { Component, OnInit } from '@angular/core';
import { InformationMessage } from 'src/app/_models/information-message';
import { InformationMessageService } from 'src/app/_services/information-message.service';
import { AlertService } from 'src/app/_services/alert.service';
import * as $ from 'jquery';
import 'datatables.net';
import 'datatables.net-bs4';
import { ErrorResponse } from 'src/app/_models/error-response';

@Component({
    selector: 'app-information-messages',
    templateUrl: './information-messages.component.html',
    styleUrls: ['./information-messages.component.css']
})
export class InformationMessagesComponent implements OnInit {
    informationMessages: InformationMessage[];
    private tableWidget: any;

    constructor(
        private informationMessageService: InformationMessageService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.informationMessageService.getAll().subscribe(
            (informationMessages) => {
                this.informationMessages = informationMessages.sort((a, b) => {
                    return <any>new Date(b.dateCreated) - <any>new Date(a.dateCreated);
                });

                this.reInitDatatable();
            },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
            }
        );
    }

    ngAfterViewInit() {
        this.initDatatable();
    }

    private initDatatable(): void {
        let table: any = $('#dataTable');
        this.tableWidget = table.DataTable();
    }

    private reInitDatatable(): void {
        if (this.tableWidget) {
            this.tableWidget.destroy();
            this.tableWidget = null;
        }

        setTimeout(this.initDatatable, 0);
    }
}
