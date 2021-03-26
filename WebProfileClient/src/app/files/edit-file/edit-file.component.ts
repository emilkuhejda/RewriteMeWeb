import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { FileItemService } from 'src/app/_services/file-item.service';
import { AlertService } from 'src/app/_services/alert.service';
import { FileItem } from 'src/app/_models/file-item';
import { ErrorResponse } from 'src/app/_models/error-response';
import { ErrorCode } from 'src/app/_enums/error-code';
import { LanguageHelper } from 'src/app/_helpers/language-helper';
import { takeUntil } from 'rxjs/operators';
import { NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';
import { HttpEventType, HttpResponse } from '@angular/common/http';
import { Subject } from 'rxjs';

@Component({
    selector: 'app-edit-file',
    templateUrl: './edit-file.component.html',
    styleUrls: ['./edit-file.component.css']
})
export class EditFileComponent implements OnInit, OnDestroy {
    private destroy$: Subject<void> = new Subject<void>();

    fileItem: FileItem;
    editFileForm: FormGroup;
    progress: number;
    selectedFileName: string = 'Choose file';
    selectedLanguage: string = '';
    submitted: boolean;
    loading: boolean;
    isModelSupported: boolean = false;

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private fileItemService: FileItemService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.editFileForm = this.formBuilder.group({
            name: ['', Validators.required],
            language: ['', Validators.required],
            audioType: ['', Validators.required],
            isTimeFrame: [0],
            startTime: [{ hour: 0, minute: 0, second: 0 }],
            endTime: [{ hour: 0, minute: 0, second: 0 }]
        });

        this.route.paramMap.pipe(takeUntil(this.destroy$)).subscribe(paramMap => {
            let fileId = paramMap.get("fileId");
            this.fileItemService.get(fileId).subscribe(
                (fileItem: FileItem) => {
                    const isTimeFrame = fileItem.transcriptionStartTime.ticks > 0 || fileItem.transcriptionEndTime.ticks > 0;

                    this.fileItem = fileItem;
                    this.selectedFileName = fileItem.fileName;
                    this.editFileForm.controls.name.setValue(fileItem.name);
                    this.editFileForm.controls.language.setValue(fileItem.language);
                    this.editFileForm.controls.audioType.setValue(fileItem.isPhoneCall ? '1' : '0');
                    this.isModelSupported = LanguageHelper.isPhoneCallModelSupported(this.controls.language.value);

                    this.editFileForm.controls.isTimeFrame.setValue(isTimeFrame);
                    if (isTimeFrame) {
                        this.editFileForm.controls.startTime.setValue(this.parseTime(fileItem.transcriptionStartTime.getTime()));
                    }

                    if (isTimeFrame) {
                        this.editFileForm.controls.endTime.setValue(this.parseTime(fileItem.transcriptionEndTime.getTime()));
                    }
                },
                (err: ErrorResponse) => {
                    this.alertService.error(err.message);
                });
        });
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.unsubscribe();
    }

    onSelectChange() {
        this.isModelSupported = LanguageHelper.isPhoneCallModelSupported(this.controls.language.value);
    }

    get controls() {
        return this.editFileForm.controls;
    }

    onChange(files) {
        if (files.length === 0)
            return;

        this.selectedFileName = "";
        this.selectedFileName = files[0].name;
    }

    onSubmit(files) {
        this.alertService.clear();
        if (this.controls.language.value === "") {
            this.alertService.error("Language is required");
            return;
        }

        if (this.controls.audioType.value === "") {
            this.alertService.error("Audio type is required")
            return;
        }

        this.submitted = true;
        if (this.editFileForm.invalid)
            return;

        this.loading = true;

        let language = this.controls.language.value;
        let audioType = LanguageHelper.isPhoneCallModelSupported(language) ? this.controls.audioType.value : 0;

        const isTimeFrame = this.controls.isTimeFrame.value;
        const startTime = this.convertToSeconds(this.controls.startTime.value);
        const endTime = this.convertToSeconds(this.controls.endTime.value);

        let formData = new FormData();
        formData.append("fileItemId", this.fileItem.id);
        formData.append("name", this.controls.name.value);
        formData.append("language", language);
        formData.append("isPhoneCall", String(audioType == 1));
        formData.append("startTimeSeconds", String(isTimeFrame ? startTime : 0));
        formData.append("endTimeSeconds", String(isTimeFrame ? endTime : 0));

        if (files.length > 0) {
            let file = files[0];
            formData.append("file", file);
            formData.append("fileName", file.name);
        }

        this.fileItemService.update(formData).subscribe(
            (event) => {
                if (event.type == HttpEventType.UploadProgress) {
                    this.progress = Math.round(100 * event.loaded / event.total);
                } else if (event instanceof HttpResponse) {
                    this.alertService.success("File was successfully updated", true);

                    this.submitted = false;
                    this.loading = false;

                    this.router.navigate(["/files"]);
                }
            },
            (err: ErrorResponse) => {
                let error = err.message;
                if (err.errorCode === ErrorCode.EC101)
                    error = "File was not found";

                if (err.errorCode === ErrorCode.EC200)
                    error = "Language is not supported";

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
            });
    }

    private convertToSeconds(timeStruct: NgbTimeStruct): number {
        return (timeStruct.hour * 3600) + (timeStruct.minute * 60) + timeStruct.second;
    }

    private createEmpty(): NgbTimeStruct {
        return { hour: 0, minute: 0, second: 0 };
    }

    private parseTime(time: string): NgbTimeStruct {
        var timeArr = time.split(':');
        if (timeArr.length < 2)
            return this.createEmpty();

        return {
            hour: Number(timeArr[0]),
            minute: Number(timeArr[1]),
            second: Number(timeArr[2])
        }
    }
}
