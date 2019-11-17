import { FileItem } from '../_models/file-item';
import { RecognitionState } from '../_enums/recognition-state';
import { TimeSpanWrapper } from '../_models/time-span-wrapper';
import { TimeoutError } from 'rxjs';
import { TimeSpanWrapperMapper } from './time-span-wrapper-mapper';

export class FileItemMapper {
    public static convertAll(data): FileItem[] {
        let files = [];
        for (let item of data) {
            let fileItem = FileItemMapper.convert(item);

            files.push(fileItem);
        }

        return files;
    }

    public static convert(data): FileItem {
        if (data === null || data === undefined)
            return null;

        let fileItem = new FileItem();
        fileItem.id = data.id;
        fileItem.name = data.name;
        fileItem.fileName = data.fileName;
        fileItem.language = data.language;
        fileItem.recognitionState = <RecognitionState>RecognitionState[<string>data.recognitionStateString];
        fileItem.totalTime = new TimeSpanWrapper(data.totalTimeTicks).getTime();
        fileItem.transcribedTime = new TimeSpanWrapper(data.transcribedTimeTicks).getTime();
        fileItem.dateCreated = new Date(data.dateCreated);
        fileItem.dateProcessed = new Date(data.dateProcessedUtc);
        fileItem.dateUpdated = new Date(data.dateUpdatedUtc);
        fileItem.isDeleted = data.isDeleted;

        return fileItem;
    }
}
