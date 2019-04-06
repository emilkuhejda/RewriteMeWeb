import { FileItem } from "../_models/file-item";

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
        let fileItem = new FileItem();
        fileItem.id = data.id;
        fileItem.name = data.name;
        fileItem.fileName = data.fileName;
        fileItem.language = data.language;
        fileItem.recognitionState = data.recognitionState;
        fileItem.dateCreated = data.dateCreated;
        fileItem.dateProcessed = data.dateProcessed;

        return fileItem;
    }
}
