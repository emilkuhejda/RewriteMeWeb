import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FileItemService } from 'src/app/_services/file-item.service';
import { AlertService } from 'src/app/_services/alert.service';
import { FileItem } from 'src/app/_models/file-item';
import { ErrorResponse } from 'src/app/_models/error-response';
import { TranscribeItemService } from 'src/app/_services/transcribe-item.service';
import { TranscribeItem } from 'src/app/_models/transcribe-item';
import { CommonVariables } from 'src/app/_config/common-variables';
import { DomSanitizer } from '@angular/platform-browser';
import { TranscribeItemViewModel } from 'src/app/_viewModels/transcribe-item-view-model';

@Component({
    selector: 'app-detail-file',
    templateUrl: './detail-file.component.html',
    styleUrls: ['./detail-file.component.css']
})
export class DetailFileComponent implements OnInit {
    fileItemName: string;
    transcribeItems: TranscribeItemViewModel[] = [];

    constructor(
        private route: ActivatedRoute,
        private fileItemService: FileItemService,
        private transcribeItemService: TranscribeItemService,
        private alertService: AlertService,
        private sanitizer: DomSanitizer) { }

    ngOnInit() {
        this.initializeFileItem().then((fileItem: FileItem) => {
            this.fileItemName = fileItem.name;
            this.initializeTranscribeItems(fileItem.id);
        });
    }

    initializeFileItem() {
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

    initializeTranscribeItems(fileItemId: string) {
        this.transcribeItemService.getAll(fileItemId).subscribe(
            (transcribeItems: TranscribeItem[]) => {
                for (let transcribeItem of transcribeItems) {
                    let viewModel = new TranscribeItemViewModel();
                    viewModel.transcribeItem = transcribeItem;
                    this.transcribeItems.push(viewModel);
                }
            },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
            }
        );
    }

    update(viewModel: TranscribeItemViewModel) {
        viewModel.isLoading = true;

        let formData = new FormData();
        formData.append("transcribeItemId", viewModel.transcribeItem.id);
        formData.append("applicationId", CommonVariables.ApplicationId);
        formData.append("transcript", viewModel.transcribeItem.userTranscript);

        this.transcribeItemService.updateTranscript(formData).subscribe(
            () => { },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
            })
            .add(() => viewModel.isLoading = false);
    }

    loadAudioFile(viewModel: TranscribeItemViewModel) {
        viewModel.isLoading = true;

        this.transcribeItemService.getAudio(viewModel.transcribeItem.id).subscribe(
            data => {
                viewModel.source = this.sanitizer.bypassSecurityTrustUrl(URL.createObjectURL(data));
            },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
            }).add(() => {
                viewModel.isLoading = false;
            });
    }
}
