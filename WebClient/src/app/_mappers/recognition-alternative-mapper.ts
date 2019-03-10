import { RecognitionAlternative } from '../_models/recognition-alternative';

export class RecognitionAlternativeMapper {
    public static convert(recognitionAlternatives): RecognitionAlternative[] {
        if (recognitionAlternatives === null || recognitionAlternatives === undefined)
            return [];

        let data = [];
        for (let alternative of recognitionAlternatives) {
            let recognitionAlternative = new RecognitionAlternative();
            recognitionAlternative.transcript = alternative.transcript;
            recognitionAlternative.confidence = alternative.confidence;

            data.push(recognitionAlternative);
        }

        return data;
    }
}
