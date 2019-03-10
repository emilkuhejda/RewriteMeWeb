import { RecognitionState } from '../_enums/recognition-state';
import { TranscribeItem } from './transcribe-item';

export class FileItem {
    public id: string;
    public name: string;
    public fileName: string;
    public recognitionState: RecognitionState;
    public dateCreated: string;
    public dateProcessed: string;
    public transcribeItems: TranscribeItem[];

    get CanUploadFile() {
        return this.recognitionState == RecognitionState.None;
    }
}
