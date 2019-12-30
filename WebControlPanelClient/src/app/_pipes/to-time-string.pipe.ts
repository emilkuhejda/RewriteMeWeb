import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'toTimeString'
})
export class ToTimeStringPipe implements PipeTransform {
    transform(value: any, ...args: any[]): any {
        var hours = value.hours;
        var minutes = value.minutes;
        var seconds = value.seconds;

        return ('0' + hours).slice(-2) + ":" + ('0' + minutes).slice(-2) + ":" + ('0' + seconds).slice(-2);
    }
}
