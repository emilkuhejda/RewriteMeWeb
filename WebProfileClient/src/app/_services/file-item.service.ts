import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonVariables } from '../_config/common-variables';
import { FileItem } from '../_models/file-item';
import { map } from 'rxjs/operators';
import { FileItemMapper } from '../_mappers/file-item-mapper';

@Injectable({
    providedIn: 'root'
})
export class FileItemService {
    constructor(private http: HttpClient) { }

    getAll() {
        var dateTime = new Date().toUTCString();

        console.log(dateTime);

        return this.http.get<FileItem[]>(CommonVariables.ApiUrl + CommonVariables.ApiFileItemsPath + "?updatedAfter=" + dateTime).pipe(map(FileItemMapper.convertAll));
    }
}
