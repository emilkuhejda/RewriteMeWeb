export class TimeSpanWrapper {
    ticks: number = 0;

    constructor(ticks: number = 0) {
        this.ticks = ticks;
    }

    getTime(): string {
        return this.getDate().toLocaleTimeString();
    }

    getDate(): Date {
        let ticksToMicrotime = this.ticks / 10000;
        let epochMicrotimeDiff = Math.abs(new Date(0, 0, 1).setFullYear(1));

        return new Date(ticksToMicrotime - epochMicrotimeDiff);
    }
}
