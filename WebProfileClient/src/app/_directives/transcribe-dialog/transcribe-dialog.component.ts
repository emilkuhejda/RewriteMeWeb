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
    private totalTime: NgbTimeStruct;
    private totalTimeSeconds: number = 0;

    public startTime: NgbTimeStruct;
    public endTime: NgbTimeStruct;
    public title: string;
    public message: string;
    public loading: boolean;
    public isTimeFrame: number = 0;

    public constructor(
        @Inject(GECO_DATA_DIALOG) public data: any,
        @Inject(GECO_DIALOG_REF) public dialogRef: GecoDialogRef) {
        this.title = data.title;
        this.message = data.message;
        this.onAccept = data.onAccept;

        let endTime = this.parseTime(data.totalTime);

        this.isTimeFrame = data.transcriptionStartTime.ticks > 0 || data.transcriptionEndTime.ticks > 0 ? 1 : 0;
        this.startTime = this.isTimeFrame ? this.parseTime(data.transcriptionStartTime.getTime()) : this.createEmpty();
        this.endTime = this.isTimeFrame ? this.parseTime(data.transcriptionEndTime.getTime()) : endTime;
        this.totalTime = endTime;
        this.totalTimeSeconds = this.convertToSeconds(this.totalTime);
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

    public getStartTimeSeconds(): number {
        return this.convertToSeconds(this.startTime);
    }

    public getEndTimeSeconds(): number {
        return this.convertToSeconds(this.endTime);
    }

    private parseTime(time: string): NgbTimeStruct {
        var timeArr = time.split(':');
        if (timeArr.length < 2)
            return this.createEmpty();

        return {
            hour: Number(timeArr[0]),
            minute: Number(timeArr[1]),
            second: Number(timeArr[2])
        }
    }

    private validateTimes(): void {
        if (!this.startTime) {
            this.startTime = this.createEmpty();
        }

        if (!this.endTime) {
            this.endTime = { ...this.totalTime };
        }

        var startTimeSeconds = this.convertToSeconds(this.startTime);
        var endTimeSeconds = this.convertToSeconds(this.endTime);

        if (endTimeSeconds > this.totalTimeSeconds) {
            this.endTime = { ...this.totalTime };
            endTimeSeconds = this.totalTimeSeconds;
        }

        if (startTimeSeconds > endTimeSeconds) {
            if (endTimeSeconds > 0) {
                this.startTime = this.convertToModel(endTimeSeconds - 1);
            } else {
                this.startTime = { ...this.endTime };
            }
        }
    }

    private convertToSeconds(timeStruct: NgbTimeStruct): number {
        return (timeStruct.hour * 3600) + (timeStruct.minute * 60) + timeStruct.second;
    }

    private convertToModel(seconds: number): NgbTimeStruct {
        let hours = Math.floor(seconds / 3600);
        let minutes = Math.floor((seconds - (hours * 3600)) / 60);
        let second = seconds - (hours * 3600) - (minutes * 60);

        return {
            hour: hours,
            minute: minutes,
            second: second
        }
    }

    private createEmpty(): NgbTimeStruct {
        return { hour: 0, minute: 0, second: 0 };
    }
}
