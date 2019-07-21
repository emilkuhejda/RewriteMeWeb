import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { FileItemService } from 'src/app/_services/file-item.service';
import { AlertService } from 'src/app/_services/alert.service';
import { HttpEventType, HttpResponse } from '@angular/common/http';
import { ErrorResponse } from 'src/app/_models/error-response';

@Component({
    selector: 'app-create-file',
    templateUrl: './create-file.component.html',
    styleUrls: ['./create-file.component.css']
})
export class CreateFileComponent implements OnInit {
    @ViewChild("fileInput") fileInputElement: ElementRef;

    createFileForm: FormGroup;
    progress: number;
    message: string;
    selectedFileName: string;
    submitted: boolean;
    loading: boolean;

    constructor(
        private formBuilder: FormBuilder,
        private router: Router,
        private fileItemService: FileItemService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.createFileForm = this.formBuilder.group({
            name: ['', Validators.required],
            language: ['', Validators.required]
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

        this.submitted = true;
        if (this.createFileForm.invalid)
            return;

        this.loading = true;

        let formData = new FormData();
        formData.append("name", this.controls.name.value);
        formData.append("language", this.controls.language.value);

        for (let file of files) {
            formData.append(file.name, file)
        }

        this.fileItemService.create(formData)
            .subscribe(
                event => {
                    if (event.type == HttpEventType.UploadProgress) {
                        this.progress = Math.round(100 * event.loaded / event.total);
                    } else if (event instanceof HttpResponse) {
                        this.alertService.success("Project was successfully created", true);
                        this.router.navigate(["/files"]);
                    }
                },
                (err: ErrorResponse) => {
                    let error = err.message;
                    if (err.status === 400)
                        error = "Uploaded file not found";

                    if (err.status === 406)
                        error = "Language is not supported";

                    if (err.status === 415)
                        error = "Audio file is not supported";

                    this.alertService.error(error);
                    this.loading = false;
                }
            );
    }
}
