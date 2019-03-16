import { TranscribeItem } from '../_models/transcribe-item';
import { RecognitionAlternativeMapper } from './recognition-alternative-mapper';

export class TranscribeItemMapper {
    public static convert(transcribeItems): TranscribeItem[] {
        if (transcribeItems === null || transcribeItems === undefined)
            return [];

        let data = [];
        for (let item of transcribeItems) {
            let alternatives = RecognitionAlternativeMapper.convert(item.alternatives);
            let transcript = alternatives.length > 0 ? alternatives[0].transcript : null;

            let transcribeItem = new TranscribeItem();
            transcribeItem.id = item.id;
            transcribeItem.transcript = transcript;
            transcribeItem.userTranscript = item.userTranscript === null ? transcript : item.userTranscript;
            transcribeItem.alternatives = alternatives;
            transcribeItem.totalTime = item.totalTime;
            transcribeItem.dateCreated = item.dateCreated;

            data.push(transcribeItem);
        }

        return data;
    }
}
