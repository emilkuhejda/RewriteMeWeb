import { TranscribeItem } from '../_models/transcribe-item';
import { RecognitionAlternativeMapper } from './recognition-alternative-mapper';
import { TimeSpanWrapper } from '../_models/time-span-wrapper';

export class TranscribeItemMapper {
    public static convertAll(data): TranscribeItem[] {
        if (data === null || data === undefined)
            return [];

        let transcribeItems = [];
        for (let item of data) {
            let transcribeItem = TranscribeItemMapper.convert(item);
            transcribeItems.push(transcribeItem);
        }

        return transcribeItems;
    }

    public static convert(data): TranscribeItem {
        if (data === null || data === undefined)
            return null;

        let alternatives = RecognitionAlternativeMapper.convert(data.alternatives);
        let transcript = alternatives.length > 0 ? alternatives[0].transcript : null;
        let userTranscript = data.userTranscript === null ? transcript : data.userTranscript;

        let transcribeItem = new TranscribeItem();
        transcribeItem.id = data.id;
        transcribeItem.fileItemId = data.fileItemId;
        transcribeItem.alternatives = alternatives;
        transcribeItem.transcript = transcript;
        transcribeItem.userTranscript = userTranscript;
        transcribeItem.startTimeString = new TimeSpanWrapper(data.startTimeTicks).getTime();
        transcribeItem.endTimeString = new TimeSpanWrapper(data.endTimeTicks).getTime();
        transcribeItem.totalTimeString = new TimeSpanWrapper(data.totalTimeTicks).getTime();
        transcribeItem.dateCreated = data.dateCreatedUtc;
        transcribeItem.dateUpdated = data.dateUpdatedUtc;

        return transcribeItem;
    }
}
