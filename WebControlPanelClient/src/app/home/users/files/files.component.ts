import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { FileItemService } from 'src/app/_services/file-item.service';
import { AlertService } from 'src/app/_services/alert.service';
import { FileItem } from 'src/app/_models/file-item';
import { ErrorResponse } from 'src/app/_models/error-response';

@Component({
    selector: 'app-files',
    templateUrl: './files.component.html',
    styleUrls: ['./files.component.css']
})
export class FilesComponent implements OnInit {
    fileItems: FileItem[] = [];
    deletedFileItems: FileItem[] = [];
    permanentlyDeletedFileItems: FileItem[] = [];

    constructor(
        private location: Location,
        private route: ActivatedRoute,
        private fileItemService: FileItemService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.route.paramMap.subscribe(paramMap => {
            let userId = paramMap.get("userId");
            this.fileItemService.getAll(userId).subscribe(
                (fileItems: FileItem[]) => {
                    fileItems.sort((a, b) => {
                        return <any>new Date(b.dateCreated) - <any>new Date(a.dateCreated);
                    });

                    this.fileItems = fileItems.filter(x => !x.isDeleted && !x.isPermanentlyDeleted);
                    this.deletedFileItems = fileItems.filter(x => x.isDeleted && !x.isPermanentlyDeleted);
                    this.permanentlyDeletedFileItems = fileItems.filter(x => x.isDeleted && x.isPermanentlyDeleted);

                    console.log(fileItems);
                },
                (err: ErrorResponse) => {
                    this.alertService.error(err.message);
                });
        });
    }

    goBack() {
        this.location.back();
    }
}
