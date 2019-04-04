import { Pipe, PipeTransform } from '@angular/core';
import { RecognitionState } from '../_enums/recognition-state';

@Pipe({
    name: 'recognitionState'
})
export class RecognitionStatePipe implements PipeTransform {
    transform(value: RecognitionState): any {
        if (value == RecognitionState.Converting)
            return "Converting";

        if (value == RecognitionState.Prepared)
            return "Prepared";

        if (value == RecognitionState.InProgress)
            return "In progress";

        if (value == RecognitionState.Completed)
            return "Completed";

        return "Prepared";
    }
}
