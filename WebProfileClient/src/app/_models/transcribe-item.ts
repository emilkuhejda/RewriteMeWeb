import { RecognitionAlternative } from './recognition-alternative';

export class TranscribeItem {
    public id: string;
    public fileItemId: string;
    public alternatives: RecognitionAlternative[];
    public userTranscript: string;
    public startTimeString: string;
    public endTimeString: string;
    public totalTimeString: string;
    public dateCreated: string;
    public dateUpdated: string;
}
