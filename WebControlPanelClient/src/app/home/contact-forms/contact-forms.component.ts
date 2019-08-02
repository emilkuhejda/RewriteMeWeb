import { Component, OnInit } from '@angular/core';
import { ContactForm } from 'src/app/_models/contact-form';
import { ContactFormService } from 'src/app/_services/contact-form.service';
import { ActivatedRoute } from '@angular/router';
import { AlertService } from 'src/app/_services/alert.service';
import { ErrorResponse } from 'src/app/_models/error-response';
import * as $ from 'jquery';
import 'datatables.net';
import 'datatables.net-bs4';

@Component({
    selector: 'app-contact-forms',
    templateUrl: './contact-forms.component.html',
    styleUrls: ['./contact-forms.component.css']
})
export class ContactFormsComponent implements OnInit {
    private tableWidget: any;
    contactForms: ContactForm[];

    constructor(
        private route: ActivatedRoute,
        private contactFormService: ContactFormService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.contactFormService.getAll().subscribe(
            (contactForms: ContactForm[]) => {
                this.contactForms = contactForms.sort((a, b) => {
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
