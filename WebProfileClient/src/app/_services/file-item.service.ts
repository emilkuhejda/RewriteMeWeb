import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
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
        let params = new HttpParams();
        params = params.append('applicationId', CommonVariables.ApplicationId);
        return this.http.get<FileItem[]>(CommonVariables.ApiUrl + CommonVariables.ApiFileItemsPath, { params: params }).pipe(map(FileItemMapper.convertAll));
    }
}
