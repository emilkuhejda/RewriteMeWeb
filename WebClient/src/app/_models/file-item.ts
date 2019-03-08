import { RecognitionState } from '../_enums/recognition-state';

export class FileItem {
    public id: string;
    public name: string;
    public fileName: string;
    public recognitionState: RecognitionState;
    public dateCreated: string;
    public dateProcessed: string;

    get CanUploadFile() {
        return this.recognitionState == RecognitionState.None;
    }
}
