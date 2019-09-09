import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { FileItemService } from 'src/app/_services/file-item.service';
import { AlertService } from 'src/app/_services/alert.service';
import { FileItem } from 'src/app/_models/file-item';
import { ErrorResponse } from 'src/app/_models/error-response';
import { GecoDialog } from 'angular-dynamic-dialog';
import { DialogComponent } from 'src/app/_directives/dialog/dialog.component';

@Component({
    selector: 'app-files',
    templateUrl: './files.component.html',
    styleUrls: ['./files.component.css']
})
export class FilesComponent implements OnInit {
    private userId: string;
    fileItems: FileItem[] = [];
    deletedFileItems: FileItem[] = [];
    permanentlyDeletedFileItems: FileItem[] = [];

    constructor(
        private location: Location,
        private route: ActivatedRoute,
        private fileItemService: FileItemService,
        private alertService: AlertService,
        private modal: GecoDialog) { }

    ngOnInit() {
        this.route.paramMap.subscribe(paramMap => {
            this.userId = paramMap.get("userId");
            this.initialize();
        });
    }

    goBack() {
        this.location.back();
    }

    restore(fileItem: FileItem) {
        this.alertService.clear();
        let onAccept = (dialogComponent: DialogComponent) => {
            this.fileItemService.restore(this.userId, fileItem.id).subscribe(
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

    private initialize(): void {
        this.fileItemService.getAll(this.userId).subscribe(
            (fileItems: FileItem[]) => {
                fileItems.sort((a, b) => {
                    return <any>new Date(b.dateCreated) - <any>new Date(a.dateCreated);
                });

                this.fileItems = fileItems.filter(x => !x.isDeleted && !x.isPermanentlyDeleted);
                this.deletedFileItems = fileItems.filter(x => x.isDeleted && !x.isPermanentlyDeleted);
                this.permanentlyDeletedFileItems = fileItems.filter(x => x.isDeleted && x.isPermanentlyDeleted);
            },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
            });
    }
}
