import { Component, OnInit, Inject } from '@angular/core';
import { GECO_DATA_DIALOG, GECO_DIALOG_REF, GecoDialogRef } from 'angular-dynamic-dialog';
import { ExportAsConfig, ExportAsService } from 'ngx-export-as';

@Component({
    selector: 'app-export-dialog',
    templateUrl: './export-dialog.component.html',
    styleUrls: ['./export-dialog.component.css']
})
export class ExportDialogComponent implements OnInit {
    private fileName: string;
    private items: any;

    loading: boolean;

    exportAsConfig: ExportAsConfig = {
        type: 'docx',
        elementId: 'export-area'
    };

    constructor(
        @Inject(GECO_DATA_DIALOG) public data: any,
        @Inject(GECO_DIALOG_REF) public dialogRef: GecoDialogRef,
        private exportAsService: ExportAsService) {
        this.fileName = data.fileName;
        this.items = data.items;
    }

    ngOnInit() { }

    accept() {
        if (this.loading)
            return;

        this.loading = true;
        this.exportAsService.save(this.exportAsConfig, this.fileName).subscribe();
        this.loading = false;
    }

    close() {
        this.loading = false;
        this.dialogRef.closeModal();
    }
}
