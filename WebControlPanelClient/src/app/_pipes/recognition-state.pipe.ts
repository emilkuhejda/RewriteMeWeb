import { Pipe, PipeTransform } from '@angular/core';
import { RecognitionState } from '../_enums/recognition-state';

@Pipe({
    name: 'recognitionState'
})
export class RecognitionStatePipe implements PipeTransform {
    transform(value: RecognitionState): any {
        if (RecognitionState[value] == RecognitionState.Converting.toString())
            return "Converting";

        if (RecognitionState[value] == RecognitionState.Prepared.toString())
            return "Prepared";

        if (RecognitionState[value] == RecognitionState.InProgress.toString())
            return "In progress";

        if (RecognitionState[value] == RecognitionState.Completed.toString())
            return "Completed";

        return "None";
    }
}
