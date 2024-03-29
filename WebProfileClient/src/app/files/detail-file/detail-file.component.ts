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
import { SendToMailDialogComponent } from 'src/app/_directives/send-to-mail-dialog/send-to-mail-dialog.component';

@Component({
    selector: 'app-detail-file',
    templateUrl: './detail-file.component.html',
    styleUrls: ['./detail-file.component.css']
})
export class DetailFileComponent implements OnInit {
    fileItem: FileItem;
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
            this.fileItem = fileItem;
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
        if (transcribeItem.isLoading || !transcribeItem.isDirty)
            return;

        transcribeItem.isLoading = true;

        let data = {
            transcribeItemId: transcribeItem.transcribeItemId,
            applicationId: CommonVariables.ApplicationId,
            transcript: transcribeItem.userTranscript
        };

        this.transcribeItemService.updateTranscript(data).subscribe(
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
        if (transcribeItem.isLoading)
            return;

        this.alertService.clear();
        if (transcribeItem.wasCleaned) {
            this.alertService.error('Audio file is not available, please use mobile application.');
            return;
        }

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
        if (transcribeItem.isLoading)
            return;

        transcribeItem.refreshTranscript();
    }

    onChange(transcribeItem: TranscribeItemViewModel) {
        transcribeItem.updateUserTranscript();
    }

    sendToMail() {
        if (this.fileItem === undefined)
            return;

        let modal = this.modal.openDialog(SendToMailDialogComponent, {
            data: {
                fileItemId: this.fileItem.id
            },
            useStyles: 'none'
        });

        modal.onClosedModal().subscribe();
    }

    restrictAudio(event) {
        const stopAudioAfter: number = 57.8;
        if (event.currentTime >= stopAudioAfter) {
            event.pause();
            event.currentTime = 0;
        }
    }
}
