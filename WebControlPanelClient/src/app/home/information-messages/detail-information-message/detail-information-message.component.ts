import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { InformationMessageService } from 'src/app/_services/information-message.service';
import { AlertService } from 'src/app/_services/alert.service';
import { InformationMessage } from 'src/app/_models/information-message';
import { ErrorResponse } from 'src/app/_models/error-response';
import { first } from 'rxjs/operators';
import { Language } from 'src/app/_enums/language';
import { LanguageVersion } from 'src/app/_models/language-version';
import { RuntimePlatform } from 'src/app/_enums/runtime-platform';
import { forkJoin } from 'rxjs';

@Component({
    selector: 'app-detail-information-message',
    templateUrl: './detail-information-message.component.html',
    styleUrls: ['./detail-information-message.component.css']
})
export class DetailInformationMessageComponent implements OnInit {
    private informationMessageId: string;
    editForm: FormGroup;
    englishVersion: LanguageVersion;
    slovakVersion: LanguageVersion;
    sendingNotification: boolean;
    loading: boolean = false;
    submitted: boolean = false;
    canUpdate: boolean = false;
    canToSendAll: boolean = false;

    constructor(
        private formBuilder: FormBuilder,
        private router: Router,
        private route: ActivatedRoute,
        private informationMessageService: InformationMessageService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.editForm = this.formBuilder.group({
            campaignName: ['', [Validators.required, Validators.maxLength(150)]],
            titleEn: ['', [Validators.required, Validators.maxLength(150)]],
            titleSk: ['', [Validators.required, Validators.maxLength(150)]],
            messageEn: ['', [Validators.required, Validators.maxLength(250)]],
            messageSk: ['', [Validators.required, Validators.maxLength(250)]],
            descriptionEn: ['', Validators.required],
            descriptionSk: ['', Validators.required]
        });

        this.route.paramMap.subscribe(paramMap => {
            this.informationMessageId = paramMap.get("informationMessageId");
            this.initializeInformationMessage();
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
                    let error = err.message;
                    if (err.status === 400)
                        error = "Push notification cannot be updated because they were already sent.";

                    this.alertService.error(error);
                    this.loading = false;
                });
    }

    sendToAll() {
        this.loading = true;

        let responseData1 = this.sendNotificationInternal(this.englishVersion, RuntimePlatform.Android);
        let responseData2 = this.sendNotificationInternal(this.englishVersion, RuntimePlatform.Osx);
        let responseData3 = this.sendNotificationInternal(this.slovakVersion, RuntimePlatform.Android);
        let responseData4 = this.sendNotificationInternal(this.slovakVersion, RuntimePlatform.Osx);
        let responses = [];

        if (responseData1 != null) {
            responses.push(responseData1);
        }

        if (responseData2 != null) {
            responses.push(responseData2);
        }

        if (responseData3 != null) {
            responses.push(responseData3);
        }

        if (responseData4 != null) {
            responses.push(responseData4);
        }

        if (responses.length == 0)
            return;

        forkJoin(responses)
            .subscribe(() => this.initializeInformationMessage())
            .add(() => {
                this.sendingNotification = false;
                this.loading = false;
            });
    }

    sendNotification(languageVersion: LanguageVersion, runtimePlatform: RuntimePlatform) {
        let pushNotificationWasSentErrorMessage = "Push notification was sent on that platform.";

        var result = this.sendNotificationInternal(languageVersion, runtimePlatform);
        if (result == null) {
            this.alertService.error(pushNotificationWasSentErrorMessage);
            this.sendingNotification = false;
            return;
        }

        result.pipe(first())
            .subscribe(
                (informationMessage: InformationMessage) => {
                    this.initialize(informationMessage);
                    this.alertService.success("Push notification was successfully sent.");
                },
                (err: ErrorResponse) => {
                    let error = err.message;

                    if (err.status === 404)
                        error = "Language version not found.";

                    if (err.status === 406)
                        error = "Language is not supported.";

                    if (err.status === 409)
                        error = pushNotificationWasSentErrorMessage;

                    this.alertService.error(error);
                })
            .add(() => this.sendingNotification = false);
    }

    private sendNotificationInternal(languageVersion: LanguageVersion, runtimePlatform: RuntimePlatform) {
        let pushNotificationWasSentErrorMessage = "Push notification was sent on that platform.";
        this.sendingNotification = true;

        if (runtimePlatform == RuntimePlatform.Android) {
            if (languageVersion.sentOnAndroid) {
                this.alertService.error(pushNotificationWasSentErrorMessage);
                this.sendingNotification = false;
                return null;
            }
        }

        if (runtimePlatform == RuntimePlatform.Osx) {
            if (languageVersion.sentOnOsx) {
                this.alertService.error(pushNotificationWasSentErrorMessage);
                this.sendingNotification = false;
                return null;
            }
        }

        let formData = new FormData();
        formData.append('informationMessageId', this.informationMessageId);
        formData.append('runtimePlatform', runtimePlatform.toString());
        formData.append('language', languageVersion.language.toString());

        return this.informationMessageService.sendNotification(formData);
    }

    private initializeInformationMessage() {
        this.informationMessageService.get(this.informationMessageId).subscribe(
            (informationMessage: InformationMessage) => {
                this.initialize(informationMessage);
            },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
            });
    }

    private initialize(informationMessage: InformationMessage) {
        this.englishVersion = informationMessage.languageVersions.find(version => version.language == Language.English);
        this.slovakVersion = informationMessage.languageVersions.find(version => version.language == Language.Slovak);

        if (this.englishVersion !== undefined) {
            this.controls.campaignName.setValue(informationMessage.campaignName);
            this.controls.titleEn.setValue(this.englishVersion.title);
            this.controls.messageEn.setValue(this.englishVersion.message);
            this.controls.descriptionEn.setValue(this.englishVersion.description);
        }

        if (this.slovakVersion !== undefined) {
            this.controls.titleSk.setValue(this.slovakVersion.title);
            this.controls.messageSk.setValue(this.slovakVersion.message);
            this.controls.descriptionSk.setValue(this.slovakVersion.description);
        }

        this.canUpdate = [this.englishVersion, this.slovakVersion].every(languageVersion => !languageVersion.sentOnAndroid && !languageVersion.sentOnOsx);
        this.canToSendAll = [this.englishVersion, this.slovakVersion].some(languageVersion => !languageVersion.sentOnAndroid || !languageVersion.sentOnOsx);
        let r = [this.englishVersion, this.slovakVersion].some(languageVersion => !languageVersion.sentOnAndroid || !languageVersion.sentOnOsx);
    }
}
