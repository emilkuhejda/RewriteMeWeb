import { Component, Inject, OnInit } from '@angular/core';
import { NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';
import { GecoDialogRef, GECO_DATA_DIALOG, GECO_DIALOG_REF } from 'angular-dynamic-dialog';

@Component({
    selector: 'app-transcribe-dialog',
    templateUrl: './transcribe-dialog.component.html',
    styleUrls: ['./transcribe-dialog.component.css']
})
export class TranscribeDialogComponent implements OnInit {
    private onAccept: Function;

    public startTime: NgbTimeStruct = { hour: 0, minute: 0, second: 0 };
    public endTime: NgbTimeStruct = { hour: 0, minute: 0, second: 0 };
    public title: string;
    public message: string;
    public loading: boolean;
    public model = 0;

    public constructor(
        @Inject(GECO_DATA_DIALOG) public data: any,
        @Inject(GECO_DIALOG_REF) public dialogRef: GecoDialogRef) {
        this.title = data.title;
        this.message = data.message;
        this.onAccept = data.onAccept;
        this.endTime = this.parseTime(data.totalTime);
    }

    public ngOnInit() { }

    public onTimeChange() {
        this.validateTimes();
    }

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

    private validateTimes(): void {
        var startTimeSeconds = this.convertToSeconds(this.startTime);
        var endTimeSeconds = this.convertToSeconds(this.endTime);

        if (startTimeSeconds > endTimeSeconds) {
            this.startTime = {
                hour: this.endTime.hour,
                minute: this.endTime.minute,
                second: this.endTime.second
            };
        }
    }

    private convertToSeconds(timeStruct: NgbTimeStruct): number {
        return (timeStruct.hour * 3600) + (timeStruct.minute * 60) + timeStruct.second;
    }
}
