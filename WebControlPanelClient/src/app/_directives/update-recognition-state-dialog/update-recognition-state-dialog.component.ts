import { Component, OnInit, Inject } from '@angular/core';
import { GecoDialogRef, GECO_DIALOG_REF, GECO_DATA_DIALOG } from 'angular-dynamic-dialog';

@Component({
    selector: 'app-update-recognition-state-dialog',
    templateUrl: './update-recognition-state-dialog.component.html',
    styleUrls: ['./update-recognition-state-dialog.component.css']
})
export class UpdateRecognitionStateDialogComponent implements OnInit {
    private onAccept: Function;
    
    fileName: string;
    loading: boolean;
    
    constructor(
        @Inject(GECO_DATA_DIALOG) public onAcceptFunction: Function,
        @Inject(GECO_DIALOG_REF) public dialogRef: GecoDialogRef) {
            this.onAccept = onAcceptFunction;
    }

    ngOnInit() {
    }

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
