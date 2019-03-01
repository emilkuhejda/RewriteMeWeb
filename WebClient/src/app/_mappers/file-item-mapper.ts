import { FileItem } from "../_models/file-item";

export class FileItemMapper {
    static convert(data): FileItem[] {
        let files = [];
        for (let item of data) {
            var fileItem = new FileItem();
            fileItem.id = item.id;
            fileItem.name = item.name;
            fileItem.dateCreated = item.dateCreated;
            fileItem.dateProcessed = item.dateProcessed;

            files.push(fileItem);
        }

        return files;
    }
}
