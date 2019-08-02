import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { FileItemService } from 'src/app/_services/file-item.service';
import { AlertService } from 'src/app/_services/alert.service';
import { FileItem } from 'src/app/_models/file-item';
import { ErrorResponse } from 'src/app/_models/error-response';

@Component({
    selector: 'app-edit-file',
    templateUrl: './edit-file.component.html',
    styleUrls: ['./edit-file.component.css']
})
export class EditFileComponent implements OnInit {
    fileItem: FileItem;
    editFileForm: FormGroup;
    submitted: boolean;
    loading: boolean;

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
                (fileItem: FileItem) => {
                    this.fileItem = fileItem;
                    this.editFileForm.controls.name.setValue(fileItem.name);
                    this.editFileForm.controls.language.setValue(fileItem.language);
                },
                (err: ErrorResponse) => {
                    this.alertService.error(err.message);
                });
        });
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

        this.submitted = true;
        if (this.editFileForm.invalid)
            return;

        this.loading = true;

        let formData = new FormData();
        formData.append("fileItemId", this.fileItem.id);
        formData.append("name", this.controls.name.value);
        formData.append("language", this.controls.language.value);

        this.fileItemService.update(formData).subscribe(
            () => {
                this.alertService.success("File was successfully created", true);
                this.router.navigate(["/files"]);
            },
            (err: ErrorResponse) => {
                let error = err.message;
                if (err.status === 406)
                    error = "Language is not supported";

                this.alertService.error(error);
                this.loading = false;
            });
    }
}
