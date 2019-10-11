import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FileItemService } from 'src/app/_services/file-item.service';
import { AlertService } from 'src/app/_services/alert.service';
import { FileItem } from 'src/app/_models/file-item';
import { ErrorResponse } from 'src/app/_models/error-response';
import { TranscribeItemService } from 'src/app/_services/transcribe-item.service';
import { TranscribeItem } from 'src/app/_models/transcribe-item';
import { CommonVariables } from 'src/app/_config/common-variables';
import { DomSanitizer } from '@angular/platform-browser';
import { TranscribeItemViewModel } from 'src/app/_viewModels/transcribe-item-view-model';
import { GecoDialog } from 'angular-dynamic-dialog';
import { ExportDialogComponent } from 'src/app/_directives/export-dialog/export-dialog.component';

@Component({
    selector: 'app-detail-file',
    templateUrl: './detail-file.component.html',
    styleUrls: ['./detail-file.component.css']
})
export class DetailFileComponent implements OnInit {
    fileItemName: string;
    transcribeItems: TranscribeItemViewModel[] = [];

    constructor(
        private route: ActivatedRoute,
        private fileItemService: FileItemService,
        private transcribeItemService: TranscribeItemService,
        private alertService: AlertService,
        private modal: GecoDialog,
        private sanitizer: DomSanitizer) { }

    ngOnInit() {
        this.initializeFileItem().then((fileItem: FileItem) => {
            this.fileItemName = fileItem.name;
            this.initializeTranscribeItems(fileItem.id);
        });
    }

    initializeFileItem() {
        return new Promise(resolve => {
            this.route.paramMap.subscribe(paramMap => {
                let fileId = paramMap.get("fileId");
                this.fileItemService.get(fileId).subscribe(
                    (fileItem: FileItem) => {
                        resolve(fileItem);
                    },
                    (err: ErrorResponse) => {
                        this.alertService.error(err.message);
                    });
            });
        });
    }

    initializeTranscribeItems(fileItemId: string) {
        this.transcribeItemService.getAll(fileItemId).subscribe(
            (transcribeItems: TranscribeItem[]) => {
                for (let transcribeItem of transcribeItems) {
                    this.transcribeItems.push(new TranscribeItemViewModel(transcribeItem));
                }
            },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
            }
        );
    }

    update(transcribeItem: TranscribeItemViewModel) {
        transcribeItem.isLoading = true;

        let formData = new FormData();
        formData.append("transcribeItemId", transcribeItem.transcribeItemId);
        formData.append("applicationId", CommonVariables.ApplicationId);
        formData.append("transcript", transcribeItem.userTranscript);

        this.transcribeItemService.updateTranscript(formData).subscribe(
            () => { },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
            })
            .add(() => {
                transcribeItem.isLoading = false;
                transcribeItem.clear();
            });
    }

    loadAudioFile(transcribeItem: TranscribeItemViewModel) {
        this.alertService.clear();
        transcribeItem.isLoading = true;

        this.transcribeItemService.getAudio(transcribeItem.transcribeItemId).subscribe(
            data => {
                transcribeItem.source = this.sanitizer.bypassSecurityTrustUrl(URL.createObjectURL(data));
            },
            (err: ErrorResponse) => {
                let error = err.message;
                if (err.status === 404)
                    error = "Audio was not found.";

                this.alertService.error(error);
            }).add(() => {
                transcribeItem.isLoading = false;
            });
    }

    refresh(transcribeItem: TranscribeItemViewModel) {
        transcribeItem.refreshTranscript();
    }

    onChange(transcribeItem: TranscribeItemViewModel) {
        transcribeItem.updateUserTranscript();
    }

    export() {
        let modal = this.modal.openDialog(ExportDialogComponent, {
            data: {
                fileName: this.fileItemName,
                transcribeItems: this.transcribeItems
            },
            useStyles: 'none'
        });

        modal.onClosedModal().subscribe();
    }
}
