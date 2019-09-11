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

            this.fileItemService.updateRecognitionState(this.fileItem.id, recognitionState).subscribe(
                (fileItem: FileItem) => {
                    if (fileItem == null) {
                        this.alertService.error("Recognition state was not changed.");
                        return;
                    }

                    this.fileItem = fileItem;
                },
                (err: ErrorResponse) => {
                    this.alertService.error(err.message);
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
