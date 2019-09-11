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
    totalSeconds: number = 0;
    time: string;
    isSelectBoxVisible: boolean;
    loading: boolean;

    constructor(
        @Inject(GECO_DATA_DIALOG) public onAcceptFunction: Function,
        @Inject(GECO_DIALOG_REF) public dialogRef: GecoDialogRef) {
        this.onAccept = onAcceptFunction;
    }

    ngOnInit() {
        this.checkVisibility();
    }

    onTimeChange() {
        this.totalSeconds = 0;
        this.checkVisibility();

        var timeArr = this.time.split(":");
        if (timeArr.length != 3) {
            return;
        }

        let hours = Number(timeArr[0]);
        let minutes = Number(timeArr[1]);
        let seconds = Number(timeArr[2]);

        if (hours === NaN || minutes === NaN || seconds === NaN) {
            return;
        }

        this.totalSeconds = (hours * 60 * 60) + (minutes * 60) + (seconds);
    }

    onMinutesChange() {
        this.totalSeconds = this.selectedMinutes * 60;
    }

    accept() {
        if (this.totalSeconds === 0)
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

    private checkVisibility(): void {
        if (this.time === "" || this.time === undefined) {
            this.isSelectBoxVisible = true;
        } else {
            this.isSelectBoxVisible = false;
        }
    }
}
