import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { FileItemService } from 'src/app/_services/file-item.service';
import { AlertService } from 'src/app/_services/alert.service';
import { HttpEventType, HttpResponse } from '@angular/common/http';
import { ErrorResponse } from 'src/app/_models/error-response';
import { FileItem } from 'src/app/_models/file-item';

@Component({
    selector: 'app-edit-file',
    templateUrl: './edit-file.component.html',
    styleUrls: ['./edit-file.component.css']
})
export class EditFileComponent implements OnInit {
    @ViewChild('fileInput') fileInputElement: ElementRef;

    fileItem: FileItem;
    editFileForm: FormGroup;
    progress: number;
    message: string;
    selectedFileName: string;
    submitted: boolean;
    loading: boolean;

    private selectedFileList: any;

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private fileItemService: FileItemService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.editFileForm = this.formBuilder.group({
            name: ['', Validators.required],
            language: ['', Validators.required]
        });

        this.route.paramMap.subscribe(paramMap => {
            let fileId = paramMap.get("fileId");
            this.fileItemService.get(fileId).subscribe(
                data => {
                    this.fileItem = data;
                    this.editFileForm.controls.name.setValue(data.name);
                    this.selectedFileName = data.fileName;

                    this.controls.language.setValue(data.language);
                },
                (err: ErrorResponse) => {
                    this.alertService.error(err.message);
                }
            );
        });
    }

    get controls() {
        return this.editFileForm.controls;
    }

    onChange(files) {
        if (files.length === 0)
            return;

        this.selectedFileName = "";
        this.selectedFileName = files[0].name;
        this.selectedFileList = files;
    }

    onSubmit() {
        this.alertService.clear();
        if (this.selectedFileList !== undefined && this.selectedFileList.length === 0) {
            this.alertService.error("File is required");
            return;
        }

        if (this.controls.language.value === "") {
            this.alertService.error("Language is required");
            return;
        }

        this.submitted = true;
        if (this.editFileForm.invalid)
            return;

        this.loading = true;

        let formData = new FormData();
        formData.append("fileItemId", this.fileItem.id);
        formData.append("name", this.controls.name.value);
        formData.append("language", this.controls.language.value);

        if (this.selectedFileList !== undefined) {
            for (let file of this.selectedFileList) {
                formData.append(file.name, file)
            }
        }

        this.fileItemService.update(formData)
            .subscribe(
                event => {
                    if (event.type == HttpEventType.UploadProgress) {
                        this.progress = Math.round(100 * event.loaded / event.total);
                    } else if (event instanceof HttpResponse) {
                        this.alertService.success("Project was successfully created", true);
                        this.router.navigate(["/admin/files"]);
                    }
                },
                (err: ErrorResponse) => {
                    let error = err.message;
                    if (err.status === 400)
                        error = "Uploaded file not found";

                    if (err.status === 416)
                        error = "Multiple upload is not allowed";

                    if (err.status === 415)
                        error = "Audio file is not supported";

                    this.alertService.error(error);
                    this.loading = false;
                });
    }
}
