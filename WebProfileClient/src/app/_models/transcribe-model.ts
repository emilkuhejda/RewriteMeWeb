export interface TranscribeModel {
    fileItemId: string;
    language: string;
    isPhoneCall: boolean;
    isTimeFrame: boolean;
    startTime: number;
    endTime: number;
}