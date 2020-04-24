import { RecognitionAlternative } from '../_models/recognition-alternative';
import { RecognitionWordInfoMapper } from './recognition-word-info-mapper';

export class RecognitionAlternativeMapper {
    public static convertAll(data): RecognitionAlternative[] {
        if (data === null || data === undefined)
            return [];

        let recognitionAlternatives = [];
        for (let alternative of data) {
            let recognitionAlternative = RecognitionAlternativeMapper.convert(alternative)
            recognitionAlternatives.push(recognitionAlternative);
        }

        return recognitionAlternatives;
    }

    public static convert(alternative): RecognitionAlternative {
        if (alternative === null || alternative === undefined)
            return null;

        let recognitionAlternative = new RecognitionAlternative();
        recognitionAlternative.transcript = alternative.transcript;
        recognitionAlternative.confidence = alternative.confidence;
        recognitionAlternative.words = RecognitionWordInfoMapper.convertAll(alternative.words);

        return recognitionAlternative;
    }
}
