import { RecognitionState } from '../_enums/recognition-state';

export class CacheItem {
    public fileItemId: string;
    public recognitionState: RecognitionState;
    public percentageDone: number;
}
