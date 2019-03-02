import { Component, OnInit } from '@angular/core';
import { ScriptLoaderService } from 'src/app/_services/script-loader.service';
import { FileItem } from 'src/app/_models/file-item';
import { FileItemService } from 'src/app/_services/file-item.service';
import { AlertService } from 'src/app/_services/alert.service';
import { ErrorResponse } from 'src/app/_models/error-response';

@Component({
    selector: 'app-files',
    templateUrl: './files.component.html',
    styleUrls: ['./files.component.css']
})
export class FilesComponent implements OnInit {
    private readonly scriptKey: string = "data-table-script";
    private readonly scriptUrl: string = "/src/assets/js/dataTable.js";

    fileItems: FileItem[];

    constructor(
        private fileItemService: FileItemService,
        private alertService: AlertService,
        private scriptLoaderService: ScriptLoaderService) { }

    ngOnInit() {
        this.fileItemService.getAll()
            .subscribe(
                data => {
                    this.fileItems = data;
                },
                (err: ErrorResponse) => {
                    this.alertService.error(err.message);
                })
            .add(
                () => {
                    this.scriptLoaderService.loadScript(this.scriptUrl, this.scriptKey);
                });
    }

    transcribe(fileId: string) {
        this.fileItemService.transcribe(fileId).subscribe(data => { }, (err: ErrorResponse) => { });
    }

    ngOnDestroy(): void {
        this.scriptLoaderService.removeScriptElement(this.scriptKey);
    }
}
