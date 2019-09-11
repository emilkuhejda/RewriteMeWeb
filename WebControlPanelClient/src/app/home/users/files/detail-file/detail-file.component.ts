import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { FileItemService } from 'src/app/_services/file-item.service';
import { FileItem } from 'src/app/_models/file-item';
import { AlertService } from 'src/app/_services/alert.service';
import { ActivatedRoute } from '@angular/router';
import { ErrorResponse } from 'src/app/_models/error-response';
import { RecognitionState } from 'src/app/_enums/recognition-state';
import { GecoDialog } from 'angular-dynamic-dialog';
import { UpdateRecognitionStateDialogComponent } from 'src/app/_directives/update-recognition-state-dialog/update-recognition-state-dialog.component';
import { CommonVariables } from 'src/app/_config/common-variables';

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
        let onAccept = (dialogComponent: UpdateRecognitionStateDialogComponent) => {
            if (dialogComponent.fileName !== this.fileItem.fileName) {
                this.alertService.error("File name must be correct.");
                dialogComponent.close();
                return;
            }

            let data = {
                fileItemId: this.fileItem.id,
                fileName: dialogComponent.fileName,
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

        let modal = this.modal.openDialog(UpdateRecognitionStateDialogComponent, {
            data: onAccept,
            useStyles: 'none'
        });

        modal.onClosedModal().subscribe();
    }
}
