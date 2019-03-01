import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { FileItemService } from 'src/app/_services/file-item.service';
import { AlertService } from 'src/app/_services/alert.service';

@Component({
    selector: 'app-create-file',
    templateUrl: './create-file.component.html',
    styleUrls: ['./create-file.component.css']
})
export class CreateFileComponent implements OnInit {
    @ViewChild('fileInput') fileInputElement: ElementRef;

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
            name: ['', Validators.required]
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
        if (files.length === 0) {
            this.alertService.error("File is required");
            return;
        }

        this.submitted = true;
        if (this.createFileForm.invalid)
            return;

        this.loading = true;

        let formData = new FormData();
        formData.append("name", this.controls.name.value);
        for (let file of files) {
            formData.append(file.name, file)
        }

        this.fileItemService.create(formData)
            .subscribe(
                () => {
                    this.alertService.success("Project was successfully created", true);
                    this.router.navigate(["/admin/files"]);
                },
                error => {
                    this.alertService.error(error);
                    this.loading = false;
                });
    }
}
