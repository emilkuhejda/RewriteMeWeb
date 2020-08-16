import { formatDate } from '@angular/common';

export class TimeSpanWrapper {
    ticks: number = 0;

    constructor(ticks: number = 0) {
        this.ticks = ticks;
    }

    getTime(): string {
        return formatDate(this.getDate(), 'HH:mm:ss', 'en-US');
    }

    getDate(): Date {
        let ticksToMicrotime = this.ticks / 10000;
        let epochMicrotimeDiff = Math.abs(new Date(0, 0, 1).setFullYear(1));

        return new Date(ticksToMicrotime - epochMicrotimeDiff);
    }
}
