import { Component, OnInit, ViewChild } from '@angular/core';
import { FileItemService } from 'src/app/_services/file-item.service';
import { TranscribeItemService } from 'src/app/_services/transcribe-item.service';
import { AlertService } from 'src/app/_services/alert.service';
import { FileItem } from 'src/app/_models/file-item';
import { ActivatedRoute } from '@angular/router';
import { ErrorResponse } from 'src/app/_models/error-response';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { TranscribeItem } from 'src/app/_models/transcribe-item';

@Component({
    selector: 'app-detail-file',
    templateUrl: './detail-file.component.html',
    styleUrls: ['./detail-file.component.css']
})
export class DetailFileComponent implements OnInit {
    fileItemName: string;
    transcribeItems: TranscribeItem[];

    constructor(
        private route: ActivatedRoute,
        private fileItemService: FileItemService,
        private transcribeItemService: TranscribeItemService,
        private alertService: AlertService,
        private sanitizer: DomSanitizer) { }

    ngOnInit() {
        this.getFileItem().then((fileItem: FileItem) => {
            this.fileItemName = fileItem.name;

            this.getTranscribeItem(fileItem.id).then((transcribeItems: TranscribeItem[]) => {
                this.transcribeItems = transcribeItems;

                for (var index in transcribeItems) {
                    let transcribeItem = transcribeItems[index];
                    this.transcribeItemService.getAudio(transcribeItem.id).subscribe(
                        data => {
                            transcribeItem.source = this.sanitizer.bypassSecurityTrustUrl(URL.createObjectURL(data));
                        }
                    )
                }
            });
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
        return new Promise(resolve => {
            this.transcribeItemService.getAll(fileItemId).subscribe(
                (transcribeItems: TranscribeItem[]) => {
                    resolve(transcribeItems);
                },
                (err: ErrorResponse) => {
                    this.alertService.error(err.message);
                }
            )
        });
    }
}
