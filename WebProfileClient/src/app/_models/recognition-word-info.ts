import { TimeSpanWrapper } from './time-span-wrapper';

export class RecognitionWordInfo {
    public word: string;
    public speakerTag: number;
    public startTime: TimeSpanWrapper;
    public endTime: TimeSpanWrapper;
}
