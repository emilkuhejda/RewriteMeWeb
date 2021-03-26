import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { FileItemService } from 'src/app/_services/file-item.service';
import { AlertService } from 'src/app/_services/alert.service';
import { HttpEventType, HttpResponse, HttpParams } from '@angular/common/http';
import { ErrorResponse } from 'src/app/_models/error-response';
import { CommonVariables } from 'src/app/_config/common-variables';
import { ErrorCode } from 'src/app/_enums/error-code';
import { LanguageHelper } from 'src/app/_helpers/language-helper';
import { Subject } from 'rxjs';

@Component({
    selector: 'app-create-file',
    templateUrl: './create-file.component.html',
    styleUrls: ['./create-file.component.css']
})
export class CreateFileComponent implements OnInit {
    createFileForm: FormGroup;
    progress: number;
    selectedFileName: string = 'Choose file';
    selectedLanguage: string = '';
    submitted: boolean;
    loading: boolean;
    isModelSupported: boolean = false;

    constructor(
        private formBuilder: FormBuilder,
        private router: Router,
        private fileItemService: FileItemService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.createFileForm = this.formBuilder.group({
            name: ['', Validators.required],
            language: ['', Validators.required],
            audioType: ['0', Validators.required]
        });
    }

    get controls() {
        return this.createFileForm.controls;
    }

    onChange(files) {
        if (files.length === 0)
            return;

        this.selectedFileName = "";
        this.selectedFileName = files[0].name;
    }

    onSelectChange() {
        this.isModelSupported = LanguageHelper.isPhoneCallModelSupported(this.controls.language.value);
    }

    onSubmit(files) {
        this.alertService.clear();
        if (files.length === 0) {
            this.alertService.error("File is required");
            return;
        }

        if (this.controls.language.value === "") {
            this.alertService.error("Language is required");
            return;
        }

        if (this.controls.audioType.value === "") {
            this.alertService.error("Audio type is required")
            return;
        }

        this.submitted = true;
        if (this.createFileForm.invalid)
            return;

        this.loading = true;

        let language = this.controls.language.value;
        let audioType = LanguageHelper.isPhoneCallModelSupported(language) ? this.controls.audioType.value : 0;

        let file = files[0];
        let params = new HttpParams();
        params = params.append("name", this.controls.name.value);
        params = params.append("language", language);
        params = params.append("fileName", file.name);
        params = params.append("isPhoneCall", String(audioType == 1));
        params = params.append("startTimeSeconds", '0');
        params = params.append("endTimeSeconds", '0');
        params = params.append("dateCreated", new Date().toISOString());
        params = params.append("applicationId", CommonVariables.ApplicationId);

        let formData = new FormData();
        formData.append("file", file);

        this.fileItemService.upload(formData, params)
            .subscribe(
                event => {
                    if (event.type == HttpEventType.UploadProgress) {
                        this.progress = Math.round(100 * event.loaded / event.total);
                    } else if (event instanceof HttpResponse) {
                        this.alertService.success("File was successfully created", true);

                        this.submitted = false;
                        this.loading = false;

                        this.router.navigate(["/files"]);
                    }
                },
                (err: ErrorResponse) => {
                    let error = err.message;
                    if (err.errorCode === ErrorCode.EC100)
                        error = "Uploaded file was not found";

                    if (err.errorCode === ErrorCode.EC200)
                        error = "Language is not supported";

                    if (err.errorCode === ErrorCode.EC201)
                        error = "Audio file is not supported";

                    if (err.errorCode === ErrorCode.EC203)
                        error = "Audio type is not supported";

                    if (err.errorCode === ErrorCode.EC204)
                        error = "Start time must be less than end time";

                    if (err.errorCode === ErrorCode.EC205)
                        error = "End time must be less than total time of the recording";

                    if (err.errorCode === ErrorCode.EC500)
                        error = "System is under maintenance. Please try again later.";

                    this.alertService.error(error);
                    this.loading = false;
                }
            );
    }
}
