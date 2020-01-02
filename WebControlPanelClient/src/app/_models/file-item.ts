import { RecognitionState } from '../_enums/recognition-state';
import { StorageSetting } from '../_enums/storage-setting';
import { UploadStatus } from '../_enums/upload-status';

export class FileItem {
    public id: string;
    public userId: string;
    public applicationId: string;
    public name: string;
    public fileName: string;
    public language: string;
    public recognitionState: RecognitionState;
    public originalSourceFileName: string;
    public originalSourcePath: string;
    public sourceFileName: string;
    public sourcePath: string;
    public originalContentType: string;
    public storage: StorageSetting;
    public uploadStatus: UploadStatus;
    public totalTime: any;
    public transcribedTime: string;
    public dateCreated: Date;
    public dateProcessed: Date;
    public dateUpdated: Date;
    public isDeleted: boolean;
    public isPermanentlyDeleted: boolean;
    public wasCleaned: boolean;
}
