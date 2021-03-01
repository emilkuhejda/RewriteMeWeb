import { Component, Inject, OnInit } from '@angular/core';
import { NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';
import { GecoDialogRef, GECO_DATA_DIALOG, GECO_DIALOG_REF } from 'angular-dynamic-dialog';

@Component({
    selector: 'app-transcribe-dialog',
    templateUrl: './transcribe-dialog.component.html',
    styleUrls: ['./transcribe-dialog.component.css']
})
export class TranscribeDialogComponent implements OnInit {
    private _startTime: NgbTimeStruct;
    private _endTime: NgbTimeStruct;
    private onAccept: Function;

    title: string;
    message: string;
    loading: boolean;
    model = 0;

    public get startTime(): NgbTimeStruct {
        return this._startTime;
    }

    public set startTime(startTime: NgbTimeStruct) {
        this._startTime = startTime;
    }

    public get endTime(): NgbTimeStruct {
        return this._startTime;
    }

    public set endTime(startTime: NgbTimeStruct) {
        this._startTime = startTime;
    }

    public constructor(
        @Inject(GECO_DATA_DIALOG) public data: any,
        @Inject(GECO_DIALOG_REF) public dialogRef: GecoDialogRef) {
        this.title = data.title;
        this.message = data.message;
        this.onAccept = data.onAccept;
        this.endTime = this.parseTime(data.totalTime);
    }

    public ngOnInit() { }

    public accept() {
        if (this.loading)
            return;

        this.loading = true;

        this.onAccept(this);
    }

    public close() {
        this.loading = false;
        this.dialogRef.closeModal();
    }

    private parseTime(time: string): NgbTimeStruct {
        var timeArr = time.split(':');
        if (timeArr.length < 2)
            return {} as NgbTimeStruct;

        return {
            hour: Number(timeArr[0]),
            minute: Number(timeArr[1]),
            second: Number(timeArr[2])
        }
    }
}
