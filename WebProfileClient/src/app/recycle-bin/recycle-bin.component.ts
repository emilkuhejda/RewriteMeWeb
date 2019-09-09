import { Component, OnInit } from '@angular/core';
import { FileItem } from '../_models/file-item';
import { FileItemService } from '../_services/file-item.service';
import { AlertService } from '../_services/alert.service';
import { GecoDialog } from 'angular-dynamic-dialog';
import { ErrorResponse } from '../_models/error-response';

@Component({
    selector: 'app-recycle-bin',
    templateUrl: './recycle-bin.component.html',
    styleUrls: ['./recycle-bin.component.css']
})
export class RecycleBinComponent implements OnInit {
    fileItems: FileItem[];

    constructor(
        private fileItemService: FileItemService,
        private alertService: AlertService,
        private modal: GecoDialog) { }

    ngOnInit() {
        this.initialize();
    }

    restore(fileItem: FileItem) {
        console.log(fileItem);
    }

    delete(fileItem: FileItem) {
        console.log(fileItem);
    }

    private initialize() {
        this.fileItemService.getDeletedFileItems().subscribe(
            (fileItems: FileItem[]) => {
                console.log(fileItems);
                this.fileItems = fileItems.sort((a, b) => {
                    return <any>new Date(b.dateUpdated) - <any>new Date(a.dateUpdated);
                });
            },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
            });
    }
}
