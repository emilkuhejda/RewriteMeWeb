import { RecognitionWordInfo } from './recognition-word-info';

export class RecognitionAlternative {
    public transcript: string;
    public confidence: number;
    public words: RecognitionWordInfo[];
}
