import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { FileItemService } from 'src/app/_services/file-item.service';
import { AlertService } from 'src/app/_services/alert.service';
import { FileItem } from 'src/app/_models/file-item';
import { ErrorResponse } from 'src/app/_models/error-response';
import { ErrorCode } from 'src/app/_enums/error-code';
import { LanguageHelper } from 'src/app/_helpers/language-helper';

@Component({
    selector: 'app-edit-file',
    templateUrl: './edit-file.component.html',
    styleUrls: ['./edit-file.component.css']
})
export class EditFileComponent implements OnInit {
    selectedLanguage: string = '';
    fileItem: FileItem;
    editFileForm: FormGroup;
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
            audioType: ['', Validators.required]
        });

        this.route.paramMap.subscribe(paramMap => {
            let fileId = paramMap.get("fileId");
            this.fileItemService.get(fileId).subscribe(
                (fileItem: FileItem) => {
                    this.fileItem = fileItem;
                    this.editFileForm.controls.name.setValue(fileItem.name);
                    this.editFileForm.controls.language.setValue(fileItem.language);
                    this.editFileForm.controls.audioType.setValue(fileItem.isPhoneCall ? '1' : '0');
                    this.isModelSupported = LanguageHelper.isPhoneCallModelSupported(this.controls.language.value);
                },
                (err: ErrorResponse) => {
                    this.alertService.error(err.message);
                });
        });
    }

    onSelectChange() {
        this.isModelSupported = LanguageHelper.isPhoneCallModelSupported(this.controls.language.value);
    }

    get controls() {
        return this.editFileForm.controls;
    }

    onSubmit() {
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

        let formData = new FormData();
        formData.append("fileItemId", this.fileItem.id);
        formData.append("name", this.controls.name.value);
        formData.append("language", language);
        formData.append("isPhoneCall", String(audioType == 1));

        this.fileItemService.update(formData).subscribe(
            () => {
                this.alertService.success("File was successfully created", true);
                this.router.navigate(["/files"]);
            },
            (err: ErrorResponse) => {
                let error = err.message;
                if (err.errorCode === ErrorCode.EC101)
                    error = "File was not found";

                if (err.errorCode === ErrorCode.EC200)
                    error = "Language is not supported";

                if (err.errorCode === ErrorCode.EC203)
                    error = "Audio type is not supported";

                if (err.errorCode === ErrorCode.EC500)
                    error = "System is under maintenance. Please try again later.";

                this.alertService.error(error);
                this.loading = false;
            });
    }
}
