import { Component, OnInit } from '@angular/core';
import { FileItemService } from '../_services/file-item.service';
import { ErrorResponse } from '../_models/error-response';
import { AlertService } from '../_services/alert.service';
import { FileItem } from '../_models/file-item';
import { GecoDialog } from 'angular-dynamic-dialog';
import { DialogComponent } from '../_directives/dialog/dialog.component';
import { ErrorCode } from '../_enums/error-code';
import { CachService } from '../_services/cach.service';
import { CacheItem } from '../_models/cache-item';
import { RecognitionState } from '../_enums/recognition-state';
import { TranscribeItemService } from '../_services/transcribe-item.service';
import { TranscribeItem } from '../_models/transcribe-item';
import { ExportDialogComponent } from 'src/app/_directives/export-dialog/export-dialog.component';

@Component({
    selector: 'app-files',
    templateUrl: './files.component.html',
    styleUrls: ['./files.component.css']
})
export class FilesComponent implements OnInit {
    constructor(
        private fileItemService: FileItemService,
        private transcribeItemService: TranscribeItemService,
        private alertService: AlertService,
        private cachService: CachService,
        private modal: GecoDialog) { }

    fileItems: FileItem[];

    ngOnInit() {
        this.cachService.startConnection();
        this.cachService.addListener(cacheItem => this.onMessageReceived(cacheItem, this.fileItems));

        this.initialize();
    }

    private onMessageReceived(cacheItem: CacheItem, fileItems: FileItem[]) {
        let fileItem = fileItems.find(fileItem => fileItem.id == cacheItem.fileItemId);
        if (fileItem === undefined)
            return;

        fileItem.recognitionState = cacheItem.recognitionState;
        fileItem.percentageDone = cacheItem.percentageDone;
    }

    download(fileItem: FileItem) {
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
        console.log(fileItem);
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
                        this.alertService.error(err.message);
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
        let onAccept = (dialogComponent: DialogComponent) => {
            this.fileItemService.transcribe(fileItem.id, fileItem.language)
                .subscribe(
                    () => {
                        this.alertService.success(`The file '${fileItem.name}' started processing`);
                    },
                    (err: ErrorResponse) => {
                        let error = err.message;
                        if (err.errorCode === ErrorCode.EC101)
                            error = "Audio file was not found";

                        if (err.errorCode === ErrorCode.EC200)
                            error = "Language is not supported";

                        if (err.errorCode === ErrorCode.EC300)
                            error = "Your subscription does not have enough free minutes";

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
                this.cachService.getCacheItem(fileItem.id).subscribe((cacheItem: CacheItem) => {
                    fileItem.percentageDone = cacheItem.percentageDone;
                });
            }
        }
    }
}
