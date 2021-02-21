import { RecognitionAlternative } from './recognition-alternative';

export class TranscribeItem {
    public id: string;
    public fileItemId: string;
    public alternatives: RecognitionAlternative[];
    public transcript: string;
    public userTranscript: string;
    public startTimeString: string;
    public endTimeString: string;
    public totalTimeString: string;
    public isIncomplete: boolean;
    public dateCreated: Date;
    public dateUpdated: Date;

    isUserTranscriptChanged(): boolean {
        return this.transcript !== this.userTranscript;
    }

    refreshTranscript() {
        this.userTranscript = this.transcript;
    }
}
