import { Component, OnInit } from '@angular/core';
import { ContactFormService } from 'src/app/_services/contact-form.service';
import { ActivatedRoute } from '@angular/router';
import { AlertService } from 'src/app/_services/alert.service';
import { ContactForm } from 'src/app/_models/contact-form';
import { ErrorResponse } from 'src/app/_models/error-response';

@Component({
    selector: 'app-detail-contact-form',
    templateUrl: './detail-contact-form.component.html',
    styleUrls: ['./detail-contact-form.component.css']
})
export class DetailContactFormComponent implements OnInit {
    contactForm: ContactForm;

    constructor(
        private route: ActivatedRoute,
        private contactFormService: ContactFormService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.route.paramMap.subscribe(paramMap => {
            let contactFormId = paramMap.get("contactFormId");
            this.contactFormService.get(contactFormId).subscribe(
                (contactForm: ContactForm) => {
                    this.contactForm = contactForm;
                },
                (err: ErrorResponse) => {
                    this.alertService.error(err.message);
                });
        });
    }
}
