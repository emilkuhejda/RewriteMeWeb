import { RecognitionWordInfo } from '../_models/recognition-word-info';
import { TimeSpanWrapper } from '../_models/time-span-wrapper';

export class RecognitionWordInfoMapper {
    public static convertAll(data): RecognitionWordInfo[] {
        if (data === null || data === undefined)
            return [];

        let recognitionWords = [];
        for (let wordInfo of data) {
            let recognitionWordInfo = RecognitionWordInfoMapper.convert(wordInfo);
            recognitionWords.push(recognitionWordInfo);
        }

        return recognitionWords;
    }

    public static convert(wordInfo): RecognitionWordInfo {
        if (wordInfo === null || wordInfo === undefined)
            return null;

        let recognitionWordInfo = new RecognitionWordInfo();
        recognitionWordInfo.word = wordInfo.word;
        recognitionWordInfo.startTime = new TimeSpanWrapper(wordInfo.startTime.ticks);
        recognitionWordInfo.endTime = new TimeSpanWrapper(wordInfo.endTime.ticks);
        recognitionWordInfo.speakerTag = wordInfo.speakerTag;

        return recognitionWordInfo;
    }
}
