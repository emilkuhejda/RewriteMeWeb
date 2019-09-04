import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { InformationMessageService } from 'src/app/_services/information-message.service';
import { AlertService } from 'src/app/_services/alert.service';
import { ErrorResponse } from 'src/app/_models/error-response';

@Component({
    selector: 'app-create-information-message',
    templateUrl: './create-information-message.component.html',
    styleUrls: ['./create-information-message.component.css']
})
export class CreateInformationMessageComponent implements OnInit {
    createForm: FormGroup;
    loading: boolean = false;
    submitted: boolean = false;

    constructor(
        private formBuilder: FormBuilder,
        private router: Router,
        private informationMessageService: InformationMessageService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.createForm = this.formBuilder.group({
            campaignName: ['', Validators.required],
            titleEn: ['', Validators.required],
            titleSk: ['', Validators.required],
            messageEn: ['', Validators.required],
            messageSk: ['', Validators.required],
            descriptionEn: ['', Validators.required],
            descriptionSk: ['', Validators.required]
        });
    }

    get controls() {
        return this.createForm.controls;
    }

    onSubmit() {
        this.submitted = true;

        if (this.createForm.invalid)
            return;

        this.loading = true;

        let formData = new FormData();
        formData.append('campaignName', this.controls.campaignName.value);
        formData.append('titleEn', this.controls.titleEn.value);
        formData.append('titleSk', this.controls.titleSk.value);
        formData.append('messageEn', this.controls.messageEn.value);
        formData.append('messageSk', this.controls.messageSk.value);
        formData.append('descriptionEn', this.controls.descriptionEn.value);
        formData.append('descriptionSk', this.controls.descriptionSk.value);

        this.informationMessageService.create(formData).subscribe(
            (informationMessageId) => {
                this.router.navigate(['information-messages/detail', informationMessageId]);
            },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
                this.loading = false;
            });
    }
}
