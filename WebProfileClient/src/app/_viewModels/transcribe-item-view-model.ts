import { TranscribeItem } from '../_models/transcribe-item';

export class TranscribeItemViewModel {
    readonly transcribeItemId: string;
    readonly transcript: string;
    public userTranscript: string;
    public time: string;
    public source: any;
    public isDirty: boolean;
    public isLoading: boolean;

    constructor(private transcribeItem: TranscribeItem) {
        this.transcribeItemId = transcribeItem.id;
        this.transcript = transcribeItem.transcript;
        this.userTranscript = transcribeItem.userTranscript;
        this.time = `${transcribeItem.startTimeString} - ${transcribeItem.endTimeString}`;
    }

    get isUserTranscriptChanged() {
        return this.transcribeItem.isUserTranscriptChanged();
    }

    updateUserTranscript() {
        this.isDirty = true;
        this.transcribeItem.userTranscript = this.userTranscript;
    }

    refreshTranscript() {
        this.isDirty = true;
        this.transcribeItem.refreshTranscript();

        this.userTranscript = this.transcribeItem.userTranscript;
    }

    clear() {
        this.isDirty = false;
    }
}
