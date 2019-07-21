import { Component, OnInit } from '@angular/core';
import { FileItemService } from '../_services/file-item.service';
import { ErrorResponse } from '../_models/error-response';
import { AlertService } from '../_services/alert.service';
import { FileItem } from '../_models/file-item';
import { GecoDialog } from 'angular-dynamic-dialog';
import { DialogComponent } from '../_directives/dialog/dialog.component';

@Component({
    selector: 'app-files',
    templateUrl: './files.component.html',
    styleUrls: ['./files.component.css']
})
export class FilesComponent implements OnInit {
    constructor(
        private fileItemService: FileItemService,
        private alertService: AlertService,
        private modal: GecoDialog) { }

    fileItems: FileItem[];

    ngOnInit() {
        this.initialize();
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
                        this.initialize();
                    },
                    (err: ErrorResponse) => {
                        let error = err.message;
                        if (err.status === 400)
                            error = "Audio file was not found";

                        if (err.status === 403)
                            error = "Your subscription does not have enough free minutes";

                        if (err.status === 406)
                            error = "Language is not supported";

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

    initialize() {
        this.fileItemService.getAll().subscribe(
            data => {
                this.fileItems = data.sort((a, b) => {
                    return <any>new Date(b.dateCreated) - <any>new Date(a.dateCreated);
                });
            },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
            }
        );
    }
}
