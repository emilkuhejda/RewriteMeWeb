import { Component, OnInit } from '@angular/core';
import { FileItemService } from '../_services/file-item.service';
import { ErrorResponse } from '../_models/error-response';
import { AlertService } from '../_services/alert.service';
import { MsalService } from '../_services/msal.service';
import { FileItem } from '../_models/file-item';

@Component({
    selector: 'app-files',
    templateUrl: './files.component.html',
    styleUrls: ['./files.component.css']
})
export class FilesComponent implements OnInit {
    constructor(
        private msalService: MsalService,
        private fileItemService: FileItemService,
        private alertService: AlertService) { }

    fileItems: FileItem[];
    
    ngOnInit() {
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
