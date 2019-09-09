import { RecognitionState } from '../_enums/recognition-state';

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
    public totalTime: string;
    public transcribedTime: string;
    public dateCreated: Date;
    public dateProcessed: Date;
    public dateUpdated: Date;
    public isDeleted: boolean;
    public isPermanentlyDeleted: boolean;
}
