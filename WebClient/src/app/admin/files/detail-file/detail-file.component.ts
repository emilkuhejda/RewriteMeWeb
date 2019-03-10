import { Component, OnInit } from '@angular/core';
import { FileItemService } from 'src/app/_services/file-item.service';
import { AlertService } from 'src/app/_services/alert.service';
import { FileItem } from 'src/app/_models/file-item';
import { ActivatedRoute } from '@angular/router';
import { ErrorResponse } from 'src/app/_models/error-response';

@Component({
    selector: 'app-detail-file',
    templateUrl: './detail-file.component.html',
    styleUrls: ['./detail-file.component.css']
})
export class DetailFileComponent implements OnInit {
    private fileItem: FileItem;

    constructor(
        private route: ActivatedRoute,
        private fileItemService: FileItemService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.route.paramMap.subscribe(paramMap => {
            let fileId = paramMap.get("fileId");
            this.fileItemService.get(fileId).subscribe(
                (data: FileItem) => {
                    this.fileItem = data;
                },
                (err: ErrorResponse) => {
                    this.alertService.error(err.message);
                }
            )
        });
    }
}
