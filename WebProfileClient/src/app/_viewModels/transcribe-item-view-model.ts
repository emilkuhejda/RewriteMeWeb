import { TranscribeItem } from '../_models/transcribe-item';

export class TranscribeItemViewModel {
    readonly transcribeItemId: string;
    readonly transcript: string;
    public userTranscript: string;
    public isUserTranscriptChanged: boolean;
    public source: any;
    public isDirty: boolean;
    public isLoading: boolean;

    constructor(private transcribeItem: TranscribeItem) {
        this.transcribeItemId = transcribeItem.id;
        this.transcript = transcribeItem.transcript;
        this.userTranscript = transcribeItem.userTranscript;
        this.isUserTranscriptChanged = transcribeItem.isUserTranscriptChanged();
    }

    updateUserTranscript() {
        this.isDirty = true;
        this.transcribeItem.userTranscript = this.userTranscript;
        this.isUserTranscriptChanged = this.transcribeItem.isUserTranscriptChanged();
    }

    refreshTranscript() {
        this.isDirty = true;
        this.transcribeItem.refreshTranscript();

        this.userTranscript = this.transcribeItem.userTranscript;
        this.isUserTranscriptChanged = this.transcribeItem.isUserTranscriptChanged();
    }

    clear() {
        this.isDirty = false;
    }
}
