import { Component, OnInit, OnDestroy } from '@angular/core';
import { FileItemService } from '../_services/file-item.service';
import { ErrorResponse } from '../_models/error-response';
import { AlertService } from '../_services/alert.service';
import { FileItem } from '../_models/file-item';
import { GecoDialog } from 'angular-dynamic-dialog';
import { DialogComponent } from '../_directives/dialog/dialog.component';
import { ErrorCode } from '../_enums/error-code';
import { CacheItem } from '../_models/cache-item';
import { RecognitionState } from '../_enums/recognition-state';
import { TranscribeItemService } from '../_services/transcribe-item.service';
import { TranscribeItem } from '../_models/transcribe-item';
import { ExportDialogComponent } from 'src/app/_directives/export-dialog/export-dialog.component';
import { CacheService } from '../_services/cache.service';
import { MessageCenterService } from '../_services/message-center.service';
import { SendToMailDialogComponent } from '../_directives/send-to-mail-dialog/send-to-mail-dialog.component';

@Component({
    selector: 'app-files',
    templateUrl: './files.component.html',
    styleUrls: ['./files.component.css']
})
export class FilesComponent implements OnInit, OnDestroy {
    private recognitionProgressChangedMethod: string = "recognition-progress";
    private recognitionStateChangedMethod: string = "recognition-state";
    private filesListChangedMethod: string = "file-list";

    constructor(
        private messageCenterService: MessageCenterService,
        private fileItemService: FileItemService,
        private transcribeItemService: TranscribeItemService,
        private alertService: AlertService,
        private cacheService: CacheService,
        private modal: GecoDialog) { }

    fileItems: FileItem[];

    ngOnInit() {
        this.messageCenterService.addListener(
            this.recognitionProgressChangedMethod,
            (cacheItem: CacheItem) => this.onRecognitionProgressChangedMessageReceived(cacheItem, this.fileItems));
        this.messageCenterService.addListener(
            this.recognitionStateChangedMethod,
            (fileItemId: string, recognitionState: RecognitionState) => this.onRecognitionStateChangedMessageReceived(fileItemId, recognitionState, this.fileItems));
        this.messageCenterService.addListener(
            this.filesListChangedMethod,
            () => this.onFileListChangedMessageReceived());

        this.initialize();
    }

    ngOnDestroy(): void {
        this.messageCenterService.removeListener(this.recognitionProgressChangedMethod);
        this.messageCenterService.removeListener(this.recognitionStateChangedMethod);
        this.messageCenterService.removeListener(this.filesListChangedMethod);
    }

    private onRecognitionProgressChangedMessageReceived(cacheItem: CacheItem, fileItems: FileItem[]) {
        let fileItem = fileItems.find(fileItem => fileItem.id == cacheItem.fileItemId);
        if (fileItem === undefined)
            return;

        fileItem.recognitionState = Number(RecognitionState[cacheItem.recognitionState]);
        fileItem.percentageDone = cacheItem.percentageDone;
    }

    private onRecognitionStateChangedMessageReceived(fileItemId: string, recognitionState: RecognitionState, fileItems: FileItem[]) {
        let fileItem = fileItems.find(fileItem => fileItem.id == fileItemId);
        if (fileItem === undefined)
            return;

        fileItem.recognitionState = Number(RecognitionState[recognitionState]);
    }

    private onFileListChangedMessageReceived() {
        this.initialize();
    }

    download(fileItem: FileItem) {
        return;

        this.transcribeItemService.getAll(fileItem.id).subscribe(
            (transcribeItems: TranscribeItem[]) => {
                let modal = this.modal.openDialog(ExportDialogComponent, {
                    data: {
                        fileName: fileItem.fileName,
                        transcribeItems: transcribeItems
                    },
                    useStyles: 'none'
                });

                modal.onClosedModal().subscribe();
            },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
            }
        );
    }

    sendToMail(fileItem: FileItem) {
        if (fileItem === undefined)
            return;

        let modal = this.modal.openDialog(SendToMailDialogComponent, {
            data: {
                fileItemId: fileItem.id
            },
            useStyles: 'none'
        });

        modal.onClosedModal().subscribe();
    }

    delete(fileItem: FileItem) {
        this.alertService.clear();
        let onAccept = (dialogComponent: DialogComponent) => {
            this.fileItemService.delete(fileItem.id)
                .subscribe(
                    () => {
                        this.alertService.success(`The file '${fileItem.name}' was successfully deleted`);
                        this.initialize();
                    },
                    (err: ErrorResponse) => {
                        let error = err.message;
                        if (err.errorCode === ErrorCode.EC104)
                            error = "File is not completely uploaded. Please try again later.";

                        this.alertService.error(error);
                    }
                ).add(() => {
                    dialogComponent.close();
                });
        };

        let data = {
            title: `Delete ${fileItem.name}`,
            message: `Do you really want to delete file '${fileItem.name}'?`,
            onAccept: onAccept
        };

        let modal = this.modal.openDialog(DialogComponent, {
            data: data,
            useStyles: 'none'
        });

        modal.onClosedModal().subscribe();
    }

    transcribe(fileItem: FileItem) {
        this.alertService.clear();
        if (!fileItem.CanTranscribe) {
            this.alertService.error("File is already processing");
        }

        let onAccept = (dialogComponent: DialogComponent) => {
            this.fileItemService.transcribe(fileItem.id, fileItem.language)
                .subscribe(
                    () => {
                        this.alertService.success(`The file '${fileItem.name}' started processing`);
                    },
                    (err: ErrorResponse) => {
                        let error = err.message;
                        if (err.errorCode === ErrorCode.EC101)
                            error = "Audio file was not found.";

                        if (err.errorCode === ErrorCode.EC103)
                            error = "File is already processing.";

                        if (err.errorCode === ErrorCode.EC104)
                            error = "File is not completely uploaded. Please try again later.";

                        if (err.errorCode === ErrorCode.EC200)
                            error = "Language is not supported.";

                        if (err.errorCode === ErrorCode.EC300)
                            error = "Your subscription does not have enough free minutes.";

                        if (err.errorCode === ErrorCode.EC303)
                            error = "It's only possible to transcribe one file at a time. Please wait till your transcription will be finished.";

                        if (err.errorCode === ErrorCode.EC500)
                            error = "System is under maintenance. Please try again later.";

                        this.alertService.error(error);
                    }
                ).add(() => {
                    dialogComponent.close();
                });
        };

        let data = {
            title: `Transcribe ${fileItem.name}`,
            message: `Do you really want to transcribe file '${fileItem.name}'?`,
            onAccept: onAccept
        };

        let modal = this.modal.openDialog(DialogComponent, {
            data: data,
            useStyles: 'none'
        });

        modal.onClosedModal().subscribe();
    }

    private initialize() {
        this.fileItemService.getAll().subscribe(
            (fileItems: FileItem[]) => {
                this.fileItems = fileItems.sort((a, b) => {
                    return <any>new Date(b.dateCreated) - <any>new Date(a.dateCreated);
                });

                this.updateProgress();
            },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
            }
        );
    }

    private updateProgress() {
        for (let index in this.fileItems) {
            let fileItem = this.fileItems[index];
            if (fileItem.recognitionState == RecognitionState.InProgress) {
                this.cacheService.getCacheItem(fileItem.id).subscribe((cacheItem: CacheItem) => {
                    fileItem.percentageDone = cacheItem.percentageDone;
                });
            }
        }
    }
}
