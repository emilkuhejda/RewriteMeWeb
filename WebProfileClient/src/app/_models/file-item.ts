import { RecognitionState } from '../_enums/recognition-state';

export class FileItem {
    public id: string;
    public name: string;
    public fileName: string;
    public language: string;
    public recognitionState: RecognitionState;
    public totalTime: string;
    public transcribedTime: string;
    public dateCreated: Date;
    public dateProcessed: Date;
    public dateUpdated: Date;
    public isDeleted: boolean;

    get CanTranscribe() {
        return this.recognitionState == RecognitionState.None || this.recognitionState == RecognitionState.Prepared;
    }

    get IsInProgress() {
        return this.recognitionState == RecognitionState.InProgress;
    }

    get IsCompleted() {
        return this.recognitionState == RecognitionState.Completed;
    }
}
