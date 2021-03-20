import { Component, OnInit, ViewChild, ElementRef, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { FileItemService } from 'src/app/_services/file-item.service';
import { AlertService } from 'src/app/_services/alert.service';
import { HttpEventType, HttpResponse, HttpParams } from '@angular/common/http';
import { ErrorResponse } from 'src/app/_models/error-response';
import { CommonVariables } from 'src/app/_config/common-variables';
import { ErrorCode } from 'src/app/_enums/error-code';
import { LanguageHelper } from 'src/app/_helpers/language-helper';
import { NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
    selector: 'app-create-file',
    templateUrl: './create-file.component.html',
    styleUrls: ['./create-file.component.css']
})
export class CreateFileComponent implements OnInit, OnDestroy {
    @ViewChild("fileInput") fileInputElement: ElementRef;

    private destroy$: Subject<void> = new Subject<void>();

    createFileForm: FormGroup;
    progress: number;
    selectedFileName: string = 'Choose file';
    selectedLanguage: string = '';
    submitted: boolean;
    loading: boolean;
    isModelSupported: boolean = false;
    isTimeFrame: boolean = false;

    constructor(
        private formBuilder: FormBuilder,
        private router: Router,
        private fileItemService: FileItemService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.createFileForm = this.formBuilder.group({
            name: ['', Validators.required],
            language: ['', Validators.required],
            audioType: ['0', Validators.required],
            isTimeFrame: [0],
            startTime: [{ hour: 0, minute: 0, second: 0 }],
            endTime: [{ hour: 0, minute: 0, second: 0 }]
        });

        this.createFileForm.valueChanges.pipe(takeUntil(this.destroy$)).subscribe(changes => {
            this.isTimeFrame = changes.isTimeFrame;
        })
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.unsubscribe();
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

        const isTimeFrame = this.controls.isTimeFrame.value;
        const startTime = this.convertToSeconds(this.controls.startTime.value);
        const endTime = this.convertToSeconds(this.controls.endTime.value);
        if (isTimeFrame) {
            if (startTime >= endTime) {
                this.alertService.error('Start time must be less than end time');
                return;
            }
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
        params = params.append("startTimeSeconds", String(isTimeFrame ? startTime : 0));
        params = params.append("endTimeSeconds", String(isTimeFrame ? endTime : 0));
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

    public onTimeChange() {
        this.validateTimes();
    }

    private validateTimes(): void {
        if (!this.controls.startTime.value) {
            this.controls.startTime.setValue(this.createEmpty());
        }

        if (!this.controls.endTime.value) {
            this.controls.endTime.setValue(this.createEmpty());
        }

        var startTimeSeconds = this.convertToSeconds(this.controls.startTime.value);
        var endTimeSeconds = this.convertToSeconds(this.controls.endTime.value);

        if (startTimeSeconds > endTimeSeconds) {
            if (endTimeSeconds > 0) {
                this.controls.startTime.setValue(this.convertToModel(endTimeSeconds - 1));
            } else {
                this.controls.startTime.setValue({ ...this.controls.endTime.value });
            }
        }
    }

    private convertToSeconds(timeStruct: NgbTimeStruct): number {
        return (timeStruct.hour * 3600) + (timeStruct.minute * 60) + timeStruct.second;
    }

    private convertToModel(seconds: number): NgbTimeStruct {
        let hours = Math.floor(seconds / 3600);
        let minutes = Math.floor((seconds - (hours * 3600)) / 60);
        let second = seconds - (hours * 3600) - (minutes * 60);

        return {
            hour: hours,
            minute: minutes,
            second: second
        }
    }

    private createEmpty(): NgbTimeStruct {
        return { hour: 0, minute: 0, second: 0 };
    }
}
