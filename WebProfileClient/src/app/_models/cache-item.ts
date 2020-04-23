import { RecognitionState } from '../_enums/recognition-state';

export class CacheItem {
    public fileItem: string;
    public recognitionState: RecognitionState;
    public percentageDone: number;
}
