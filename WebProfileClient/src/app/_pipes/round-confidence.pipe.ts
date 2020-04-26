import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'roundConfidence'
})
export class RoundConfidencePipe implements PipeTransform {
    transform(value: any, args?: any): any {
        return Math.round(value * 100);
    }
}
