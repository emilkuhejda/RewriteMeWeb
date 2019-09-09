import { FileItem } from '../_models/file-item';
import { RecognitionState } from '../_enums/recognition-state';

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
        fileItem.totalTime = data.totalTimeString;
        fileItem.dateCreated = data.dateCreated;
        fileItem.dateProcessed = data.dateProcessed;
        fileItem.dateUpdated = data.dateUpdated;
        fileItem.isDeleted = data.isDeleted;

        return fileItem;
    }
}
