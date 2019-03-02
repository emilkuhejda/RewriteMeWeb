import { Component, OnInit, Inject } from '@angular/core';
import { GECO_DATA_DIALOG, GECO_DIALOG_REF, GecoDialogRef } from 'angular-dynamic-dialog';
import { Observable, Observer } from 'rxjs';
import { Action } from 'rxjs/internal/scheduler/Action';

@Component({
    selector: 'app-dialog',
    templateUrl: './dialog.component.html',
    styleUrls: ['./dialog.component.css']
})
export class DialogComponent implements OnInit {
    private title: string;
    private message: string;
    private onAccept: Function;

    loading: boolean;

    constructor(
        @Inject(GECO_DATA_DIALOG) public data: any,
        @Inject(GECO_DIALOG_REF) public dialogRef: GecoDialogRef) {
        this.title = data.title;
        this.message = data.message;
        this.onAccept = data.onAccept;
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
