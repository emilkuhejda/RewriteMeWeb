import { RecognitionState } from '../_enums/recognition-state';

export class FileItem {
    public id: string;
    public name: string;
    public fileName: string;
    public language: string;
    public recognitionState: RecognitionState;
    public totalTime: string;
    public dateCreated: string;
    public dateProcessed: string;
    public dateUpdated: string;
    public audioSourceVersion: number;
    public isDeleted: boolean;
    
    get CanTranscribe() {
        return this.recognitionState == RecognitionState.None || this.recognitionState == RecognitionState.Prepared;
    }
    
    get IsInProgress() {
        return this.recognitionState == RecognitionState.InProgress;
    }
}
