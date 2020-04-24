import { FileItem } from '../_models/file-item';
import { RecognitionState } from '../_enums/recognition-state';
import { StorageSetting } from '../_enums/storage-setting';
import { UploadStatus } from '../_enums/upload-status';

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
        fileItem.userId = data.userId;
        fileItem.applicationId = data.applicationId;
        fileItem.name = data.name;
        fileItem.fileName = data.fileName;
        fileItem.language = data.language;
        fileItem.recognitionState = <RecognitionState>data.recognitionState;
        fileItem.originalSourceFileName = data.originalSourceFileName;
        fileItem.originalSourcePath = data.originalSourcePath;
        fileItem.sourceFileName = data.sourceFileName;
        fileItem.sourcePath = data.sourcePath;
        fileItem.originalContentType = data.originalContentType;
        fileItem.storage = <StorageSetting>data.storage;
        fileItem.uploadStatus = <UploadStatus>data.uploadStatus;
        fileItem.totalTime = data.totalTime;
        fileItem.transcribedTime = data.transcribedTime;
        fileItem.dateCreated = new Date(data.dateCreated);
        fileItem.dateProcessed = new Date(data.dateProcessedUtc);
        fileItem.dateUpdated = new Date(data.dateUpdatedUtc);
        fileItem.isDeleted = data.isDeleted;
        fileItem.wasCleaned = data.wasCleaned;

        return fileItem;
    }
}
