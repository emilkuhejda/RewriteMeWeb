import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FileItemService } from 'src/app/_services/file-item.service';
import { AlertService } from 'src/app/_services/alert.service';
import { FileItem } from 'src/app/_models/file-item';
import { ErrorResponse } from 'src/app/_models/error-response';
import { TranscribeItemService } from 'src/app/_services/transcribe-item.service';
import { TranscribeItem } from 'src/app/_models/transcribe-item';
import { CommonVariables } from 'src/app/_config/common-variables';

@Component({
    selector: 'app-detail-file',
    templateUrl: './detail-file.component.html',
    styleUrls: ['./detail-file.component.css']
})
export class DetailFileComponent implements OnInit {
    fileItemName: string;
    transcribeItems: TranscribeItem[];
    loading: boolean;

    constructor(
        private route: ActivatedRoute,
        private fileItemService: FileItemService,
        private transcribeItemService: TranscribeItemService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.getFileItem().then((fileItem: FileItem) => {
            this.fileItemName = fileItem.name;

            this.getTranscribeItem(fileItem.id);
        });
    }

    getFileItem() {
        return new Promise(resolve => {
            this.route.paramMap.subscribe(paramMap => {
                let fileId = paramMap.get("fileId");
                this.fileItemService.get(fileId).subscribe(
                    (fileItem: FileItem) => {
                        resolve(fileItem);
                    },
                    (err: ErrorResponse) => {
                        this.alertService.error(err.message);
                    }
                );
            });
        });
    }

    getTranscribeItem(fileItemId: string) {
        this.transcribeItemService.getAll(fileItemId).subscribe(
            (transcribeItems: TranscribeItem[]) => {
                this.transcribeItems = transcribeItems;
            },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
            }
        );
    }

    update(transcribeItem: TranscribeItem) {
        this.loading = true;

        let formData = new FormData();
        formData.append("transcribeItemId", transcribeItem.id);
        formData.append("applicationId", CommonVariables.ApplicationId);
        formData.append("transcript", transcribeItem.userTranscript);

        this.transcribeItemService.updateTranscript(formData).subscribe(
            () => { },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
            })
            .add(() => this.loading = false);
    }
}
