import { Pipe, PipeTransform } from '@angular/core';
import { UploadStatus } from '../_enums/upload-status';

@Pipe({
    name: 'uploadStatus'
})
export class UploadStatusPipe implements PipeTransform {
    transform(value: any, ...args: any[]): any {
        if (value == UploadStatus.InProgress)
            return "In progress";

        if (value == UploadStatus.Completed)
            return "Completed";

        if (value == UploadStatus.Error)
            return "Error";

        return "None";
    }
}
