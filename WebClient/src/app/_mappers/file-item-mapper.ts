import { FileItem } from "../_models/file-item";

export class FileItemMapper {
    static convertAll(data): FileItem[] {
        let files = [];
        for (let item of data) {
            let fileItem = FileItemMapper.convert(item);

            files.push(fileItem);
        }

        return files;
    }

    static convert(data): FileItem {
        let fileItem = new FileItem();
        fileItem.id = data.id;
        fileItem.name = data.name;
        fileItem.fileName = data.fileName;
        fileItem.recognitionState = data.recognitionState;
        fileItem.dateCreated = data.dateCreated;
        fileItem.dateProcessed = data.dateProcessed;

        return fileItem;
    }
}
