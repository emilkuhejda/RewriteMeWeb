import { Component, Inject, OnInit } from '@angular/core';
import { GecoDialogRef, GECO_DATA_DIALOG, GECO_DIALOG_REF } from 'angular-dynamic-dialog';

@Component({
    selector: 'app-transcribe-dialog',
    templateUrl: './transcribe-dialog.component.html',
    styleUrls: ['./transcribe-dialog.component.css']
})
export class TranscribeDialogComponent implements OnInit {
    private onAccept: Function;

    title: string;
    message: string;
    loading: boolean;

    constructor(
        @Inject(GECO_DATA_DIALOG) public data: any,
        @Inject(GECO_DIALOG_REF) public dialogRef: GecoDialogRef) {
        this.title = data.title;
        this.message = data.message;
        this.onAccept = data.onAccept;
    }

    ngOnInit() { }

    accept() {
        if (this.loading)
            return;

        this.loading = true;

        this.onAccept(this);
    }

    close() {
        this.loading = false;
        this.dialogRef.closeModal();
    }
}
