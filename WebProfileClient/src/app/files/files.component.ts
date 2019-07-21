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
