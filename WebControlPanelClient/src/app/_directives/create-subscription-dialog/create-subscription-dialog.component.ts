import { Component, OnInit, Inject } from '@angular/core';
import { GECO_DATA_DIALOG, GECO_DIALOG_REF, GecoDialogRef } from 'angular-dynamic-dialog';

@Component({
    selector: 'app-create-subscription-dialog',
    templateUrl: './create-subscription-dialog.component.html',
    styleUrls: ['./create-subscription-dialog.component.css']
})
export class CreateSubscriptionDialogComponent implements OnInit {
    private onAccept: Function;

    selectedMinutes: number = 0;
    loading: boolean;

    constructor(
        @Inject(GECO_DATA_DIALOG) public onAcceptFunction: Function,
        @Inject(GECO_DIALOG_REF) public dialogRef: GecoDialogRef) {
        this.onAccept = onAcceptFunction;
    }

    ngOnInit() { }

    accept() {
        if (this.selectedMinutes === 0)
            return;

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
