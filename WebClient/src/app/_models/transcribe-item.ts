import { RecognitionAlternative } from './recognition-alternative';

export class TranscribeItem {
    public id: string;
    public transcript: string;
    public userTranscript: string;
    public alternatives: RecognitionAlternative[];
    public source: any;
    public totalTime: string;
    public dateCreated: Date;
}
