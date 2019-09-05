import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { InformationMessageService } from 'src/app/_services/information-message.service';
import { AlertService } from 'src/app/_services/alert.service';
import { InformationMessage } from 'src/app/_models/information-message';
import { ErrorResponse } from 'src/app/_models/error-response';
import { first } from 'rxjs/operators';
import { Language } from 'src/app/_enums/language';

@Component({
    selector: 'app-detail-information-message',
    templateUrl: './detail-information-message.component.html',
    styleUrls: ['./detail-information-message.component.css']
})
export class DetailInformationMessageComponent implements OnInit {
    private informationMessageId: string;
    editForm: FormGroup;
    loading: boolean = false;
    submitted: boolean = false;

    constructor(
        private formBuilder: FormBuilder,
        private router: Router,
        private route: ActivatedRoute,
        private informationMessageService: InformationMessageService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.editForm = this.formBuilder.group({
            campaignName: ['', Validators.required],
            titleEn: ['', Validators.required],
            titleSk: ['', Validators.required],
            messageEn: ['', Validators.required],
            messageSk: ['', Validators.required],
            descriptionEn: ['', Validators.required],
            descriptionSk: ['', Validators.required]
        });

        this.route.paramMap.subscribe(paramMap => {
            this.informationMessageId = paramMap.get("informationMessageId");
            this.informationMessageService.get(this.informationMessageId).subscribe(
                (informationMessage: InformationMessage) => {
                    let englishVersion = informationMessage.languageVersions.find(version => version.language == Language.English);
                    let slovakVersion = informationMessage.languageVersions.find(version => version.language == Language.Slovak);

                    if (englishVersion !== undefined) {
                        this.controls.campaignName.setValue(informationMessage.campaignName);
                        this.controls.titleEn.setValue(englishVersion.title);
                        this.controls.messageEn.setValue(englishVersion.message);
                        this.controls.descriptionEn.setValue(englishVersion.description);
                    }

                    if (slovakVersion !== undefined) {
                        this.controls.titleSk.setValue(slovakVersion.title);
                        this.controls.messageSk.setValue(slovakVersion.message);
                        this.controls.descriptionSk.setValue(slovakVersion.description);
                    }
                },
                (err: ErrorResponse) => {
                    this.alertService.error(err.message);
                });
        });
    }

    get controls() {
        return this.editForm.controls;
    }

    onSubmit() {
        this.submitted = true;

        if (this.editForm.invalid)
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

        this.informationMessageService.update(this.informationMessageId, formData)
            .pipe(first())
            .subscribe(
                () => {
                    this.router.navigate(['/information-messages']);
                },
                (err: ErrorResponse) => {
                    this.alertService.error(err.message);
                    this.loading = false;
                });
    }
}
