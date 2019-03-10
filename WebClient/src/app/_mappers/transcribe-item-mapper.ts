import { TranscribeItem } from '../_models/transcribe-item';
import { RecognitionAlternativeMapper } from './recognition-alternative-mapper';

export class TranscribeItemMapper {
    public static convert(transcribeItems): TranscribeItem[] {
        if (transcribeItems === null || transcribeItems === undefined)
            return [];

        let data = [];
        for (let item of transcribeItems) {
            let transcribeItem = new TranscribeItem();
            transcribeItem.id = item.id;
            transcribeItem.alternatives = RecognitionAlternativeMapper.convert(item.alternatives);
            transcribeItem.source = item.source;
            transcribeItem.totalTime = item.totalTime;
            transcribeItem.dateCreated = item.dateCreated;

            data.push(transcribeItem);
        }

        return data;
    }
}
