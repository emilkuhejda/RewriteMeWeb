import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { FileItemService } from 'src/app/_services/file-item.service';
import { FileItem } from 'src/app/_models/file-item';
import { AlertService } from 'src/app/_services/alert.service';
import { ActivatedRoute } from '@angular/router';
import { ErrorResponse } from 'src/app/_models/error-response';
import { RecognitionState } from 'src/app/_enums/recognition-state';
import { GecoDialog } from 'angular-dynamic-dialog';
import { CommonVariables } from 'src/app/_config/common-variables';
import { ConfirmationDialogComponent } from 'src/app/_directives/confirmation-dialog/confirmation-dialog.component';

@Component({
    selector: 'app-detail-file',
    templateUrl: './detail-file.component.html',
    styleUrls: ['./detail-file.component.css']
})
export class DetailFileComponent implements OnInit {
    fileItem: FileItem;

    constructor(
        private location: Location,
        private route: ActivatedRoute,
        private fileItemService: FileItemService,
        private alertService: AlertService,
        private modal: GecoDialog) { }

    ngOnInit() {
        this.route.paramMap.subscribe(paramMap => {
            let fileItemId = paramMap.get("fileItemId");
            this.fileItemService.get(fileItemId).subscribe(
                (fileItem: FileItem) => {
                    this.fileItem = fileItem;
                },
                (err: ErrorResponse) => {
                    this.alertService.error(err.message);
                });
        });
    }

    goBack() {
        this.location.back();
    }

    updateRecognitionState(recognitionState: RecognitionState) {
        this.alertService.clear();
        let onAccept = (dialogComponent: ConfirmationDialogComponent) => {
            if (dialogComponent.confirmationValue !== this.fileItem.fileName) {
                this.alertService.error("File name must be correct.");
                dialogComponent.close();
                return;
            }

            let data = {
                fileItemId: this.fileItem.id,
                fileName: dialogComponent.confirmationValue,
                recognitionState: recognitionState.toString(),
                applicationId: CommonVariables.ApplicationId
            };

            this.fileItemService.updateRecognitionState(data).subscribe(
                (fileItem: FileItem) => {
                    if (fileItem == null) {
                        this.alertService.error("Recognition state was not changed.");
                        return;
                    }

                    this.fileItem = fileItem;
                },
                (err: ErrorResponse) => {
                    let error = err.message;
                    if (err.status === 400)
                        error = "File was not found.";

                    this.alertService.error(error);
                })
                .add(() => dialogComponent.close());
        };

        let data = {
            title: `Change recognition state`,
            message: `Do you really want to change recognition state?`,
            confirmationTitle: `File name`,
            onAccept: onAccept
        };

        let modal = this.modal.openDialog(ConfirmationDialogComponent, {
            data: data,
            useStyles: 'none'
        });

        modal.onClosedModal().subscribe();
    }
}
