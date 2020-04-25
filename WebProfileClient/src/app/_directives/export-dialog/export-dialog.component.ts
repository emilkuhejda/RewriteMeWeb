import { Component, OnInit, Inject } from '@angular/core';
import { GECO_DATA_DIALOG, GECO_DIALOG_REF, GecoDialogRef } from 'angular-dynamic-dialog';
import { ExportAsConfig, ExportAsService } from 'ngx-export-as';
import { TranscribeItemViewModel } from 'src/app/_viewModels/transcribe-item-view-model';

@Component({
    selector: 'app-export-dialog',
    templateUrl: './export-dialog.component.html',
    styleUrls: ['./export-dialog.component.css']
})
export class ExportDialogComponent implements OnInit {
    private exportAsPdfConfig: ExportAsConfig = {
        type: 'pdf',
        elementId: 'export-area'
    };

    private exportAsDocxConfig: ExportAsConfig = {
        type: 'docx',
        elementId: 'export-area'
    };

    fileName: string;
    transcribeItems: TranscribeItemViewModel[];

    loading: boolean;

    constructor(
        @Inject(GECO_DATA_DIALOG) public data: any,
        @Inject(GECO_DIALOG_REF) public dialogRef: GecoDialogRef,
        private exportAsService: ExportAsService) {
        this.fileName = data.fileName;
        this.transcribeItems = data.transcribeItems;
    }

    ngOnInit() { }

    saveAsPdf() {
        if (this.loading)
            return;

        this.loading = true;
        this.exportAsService.save(this.exportAsPdfConfig, this.fileName).subscribe();
        this.loading = false;
    }

    saveAsDocx() {
        if (this.loading)
            return;

        this.loading = true;
        this.exportAsService.save(this.exportAsDocxConfig, this.fileName).subscribe();
        this.loading = false;
    }

    close() {
        this.loading = false;
        this.dialogRef.closeModal();
    }
}
