import { Component, OnInit } from '@angular/core';
import { FileItem } from '../_models/file-item';
import { FileItemService } from '../_services/file-item.service';
import { AlertService } from '../_services/alert.service';
import { GecoDialog } from 'angular-dynamic-dialog';
import { ErrorResponse } from '../_models/error-response';
import { DialogComponent } from '../_directives/dialog/dialog.component';
import { ErrorCode } from '../_enums/error-code';

@Component({
    selector: 'app-recycle-bin',
    templateUrl: './recycle-bin.component.html',
    styleUrls: ['./recycle-bin.component.css']
})
export class RecycleBinComponent implements OnInit {
    fileItems: FileItem[] = [];

    constructor(
        private fileItemService: FileItemService,
        private alertService: AlertService,
        private modal: GecoDialog) { }

    ngOnInit() {
        this.initialize();
    }

    restore(fileItem: FileItem) {
        this.alertService.clear();
        let onAccept = (dialogComponent: DialogComponent) => {
            this.fileItemService.restoreAll([fileItem.id]).subscribe(
                () => {
                    this.initialize();
                },
                (err: ErrorResponse) => {
                    this.alertService.error(err.message);
                })
                .add(() => dialogComponent.close());
        };

        let data = {
            title: `Restore ${fileItem.name}`,
            message: `Do you really want to restore file '${fileItem.name}'?`,
            onAccept: onAccept
        };

        let modal = this.modal.openDialog(DialogComponent, {
            data: data,
            useStyles: 'none'
        });

        modal.onClosedModal().subscribe();
    }

    restoreAll() {
        this.alertService.clear();
        if (this.fileItems.length == 0)
            return;

        let onAccept = (dialogComponent: DialogComponent) => {
            let fileItemIds = [];
            for (let fileItem of this.fileItems) {
                fileItemIds.push(fileItem.id);
            }

            this.fileItemService.restoreAll(fileItemIds).subscribe(
                () => {
                    this.initialize();
                },
                (err: ErrorResponse) => {
                    this.alertService.error(err.message);
                })
                .add(() => dialogComponent.close());
        };

        let data = {
            title: `Restore Recycle Bin`,
            message: `Do you really want to restore all files?`,
            onAccept: onAccept
        };

        let modal = this.modal.openDialog(DialogComponent, {
            data: data,
            useStyles: 'none'
        });

        modal.onClosedModal().subscribe();
    }

    delete(fileItem: FileItem) {
        this.alertService.clear();
        let onAccept = (dialogComponent: DialogComponent) => {
            this.fileItemService.permanentDeleteAll([fileItem.id]).subscribe(
                () => {
                    this.initialize();
                },
                (err: ErrorResponse) => {
                    let error = err.message;
                    if (err.errorCode === ErrorCode.EC500)
                        error = "System is under maintenance. Please try again later.";

                    this.alertService.error(error);
                })
                .add(() => dialogComponent.close());
        };

        let data = {
            title: `Permanent delete ${fileItem.name}`,
            message: `Do you really want to permanently delete file '${fileItem.name}'?`,
            onAccept: onAccept
        };

        let modal = this.modal.openDialog(DialogComponent, {
            data: data,
            useStyles: 'none'
        });

        modal.onClosedModal().subscribe();
    }

    deleteAll() {
        this.alertService.clear();
        if (this.fileItems.length == 0)
            return;

        let onAccept = (dialogComponent: DialogComponent) => {
            let fileItemIds = [];
            for (let fileItem of this.fileItems) {
                fileItemIds.push(fileItem.id);
            }

            this.fileItemService.permanentDeleteAll(fileItemIds).subscribe(
                () => {
                    this.initialize();
                },
                (err: ErrorResponse) => {
                    let error = err.message;
                    if (err.errorCode === ErrorCode.EC500)
                        error = "System is under maintenance. Please try again later.";

                    this.alertService.error(error);
                })
                .add(() => dialogComponent.close());
        };

        let data = {
            title: `Empty Recycle Bin`,
            message: `Do you really want to permanently delete all files?`,
            onAccept: onAccept
        };

        let modal = this.modal.openDialog(DialogComponent, {
            data: data,
            useStyles: 'none'
        });

        modal.onClosedModal().subscribe();
    }

    private initialize() {
        this.fileItemService.getDeletedFileItems().subscribe(
            (fileItems: FileItem[]) => {
                this.fileItems = fileItems.sort((a, b) => {
                    return <any>new Date(b.dateUpdated) - <any>new Date(a.dateUpdated);
                });
            },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
            });
    }
}
