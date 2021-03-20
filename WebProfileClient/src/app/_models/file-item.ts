import { RecognitionState } from '../_enums/recognition-state';
import { UploadStatus } from '../_enums/upload-status';
import { TimeSpanWrapper } from './time-span-wrapper';

export class FileItem {
    public id: string;
    public name: string;
    public fileName: string;
    public language: string;
    public isPhoneCall: boolean;
    public recognitionState: RecognitionState;
    public uploadStatus: UploadStatus;
    public totalTime: string;
    public isTimeFrame: boolean;
    public transcriptionStartTime: TimeSpanWrapper;
    public transcriptionEndTime: TimeSpanWrapper;
    public transcribedTime: string;
    public dateCreated: Date;
    public dateProcessed: Date;
    public dateUpdated: Date;
    public isDeleted: boolean;
    public percentageDone: number = 0;

    get CanTranscribe() {
        return this.recognitionState == RecognitionState.None;
    }

    get IsInProgress() {
        return this.recognitionState == RecognitionState.InProgress;
    }

    get IsCompleted() {
        return this.recognitionState == RecognitionState.Completed;
    }
}
