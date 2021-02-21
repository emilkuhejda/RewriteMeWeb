import { TranscribeItem } from '../_models/transcribe-item';
import { RecognitionAlternative } from '../_models/recognition-alternative';

export class TranscribeItemViewModel {
    readonly transcribeItemId: string;
    readonly transcript: string;
    public userTranscript: string;
    public time: string;
    public isIncomplete: boolean;
    public confidence: number;
    public source: any;
    public isDirty: boolean;
    public isLoading: boolean;

    constructor(private transcribeItem: TranscribeItem) {
        this.transcribeItemId = transcribeItem.id;
        this.transcript = transcribeItem.transcript;
        this.userTranscript = transcribeItem.userTranscript;
        this.time = `${transcribeItem.startTimeString} - ${transcribeItem.endTimeString}`;
        this.isIncomplete = transcribeItem.isIncomplete;
        this.confidence = this.calculateAverageConfidence(transcribeItem.alternatives);
    }

    private calculateAverageConfidence(alternatives: RecognitionAlternative[]): number {
        if (alternatives.length <= 0)
            return 0;

        let map = alternatives.map(x => x.confidence);
        let total = map.reduce((previousValue, currentValue) => previousValue + currentValue);
        return total / map.length;
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
